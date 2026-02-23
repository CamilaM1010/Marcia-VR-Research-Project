using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using System.Diagnostics;

public class StimulusLogger : MonoBehaviour
{
    // Singleton instance - ensures only one logger exists in the scene.
    private static StimulusLogger _instance;

    // Thread-safe queue that stores log messages waiting to be written to file
    // Multiple threads can add to this queue simultaneously without conflicts
    private ConcurrentQueue<string> logQueue = new();

    // Used to signal the background trhead to stop when the simulation ends
    private CancellationTokenSource cancellationTokenSource;

    // The full file path where logs will be saved
    private string logFilePath;

    [SerializeField] private TMP_Text logLocation_TMP_Text = null;

    [SerializeField] private bool OpenExplorerOnApplicationExit = true;

    void Awake()
    {
        // Singleton pattern - if this is the first logger, keep it; otherwise destroy duplicates
        if (_instance == null)
        {
            _instance = this;

            // Keep this GameObject alive even when switching scenes (probably unnecessary)
            DontDestroyOnLoad(gameObject);

            // Create a unique log file with timestamp (e.g., "stimulus_log_20260216_161319.txt")
            logFilePath = Path.Combine(Application.persistentDataPath, $"stimulus_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            UnityEngine.Debug.Log($"STIMULUS LOG FILE DESTINATION: {logFilePath}");

            if (logLocation_TMP_Text != null) logLocation_TMP_Text.text = $"Log File Location: {logFilePath}";
            
            // Create the cancellation token (used to stop the background thread later)
            cancellationTokenSource = new CancellationTokenSource();

            // Task.Run creates a new thread that runs independently of Unity's main thread
            Task.Run(() => ProcessLogQueue(cancellationTokenSource.Token));
        }
        else 
        {
            // If a logger already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Call this static method from anywhere to add a log entry.
    /// It's thread-safe and can be called from any script.
    /// </summary>
    /// <param name="eventType">Type of event (e.g., "STIMULUS_START", "ANIMATION_START")</param>
    /// <param name="gameObjectName">Name of the GameObject involved</param>
    /// <param name="triggerSource">What triggered this event (e.g., "InputAction: XRI_RightHand_A")</param>
    /// <param name="details">Any additional information about the event</param>
    public static void Log(string eventType, string gameObjectName, string triggerSource, string details = "")
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        
        // Format the log entry as a readable string
        string logEntry = $"[{timestamp}] {eventType} | GameObject: {gameObjectName} | Trigger: {triggerSource} | {details}";
        
        // Add the log entry to the queue (the background thread will pick it up)
        if (_instance != null) _instance.logQueue.Enqueue(logEntry);
    }

    /// <summary>
    /// This runs on a background thread and continuously checks for new log entries to write.
    /// It runs independently from Unity's main thread, so it won't cause frame drops.
    /// </summary>
    /// <param name="token">Allows us to stop this thread when the game ends</param>
    private async Task ProcessLogQueue(CancellationToken token)
    {
        // Open the log file for writing
        // "using" ensure the file is proper closed even if an error occurs
        using StreamWriter writer = new(logFilePath, true);

        // Keep running until told to stop via cancellation token
        while (!token.IsCancellationRequested)
        {
            // ry to get a log entry from the queue
            if (logQueue.TryDequeue(out string logEntry))
            {
                // Write the log entry to the file
                await writer.WriteLineAsync(logEntry);

                // Force the file to save immediately (rather than waiting for the buffer to fill)
                await writer.FlushAsync();
            }
            else
            {
                // if no logs are waiting, sleep for 10ms to avoid wasting CPU
                // This creates a small delay but keeps CPU usage very low
                await Task.Delay(10, token);
            }
        }
    }

    void OnDestroy()
    {
        // Signal background thread to stop
        cancellationTokenSource?.Cancel();
    }

    void OnApplicationQuit()
    {
        if (!OpenExplorerOnApplicationExit) return;
        
        // Ensure backslashes for Windows paths
        string windowsPath = logFilePath.Replace("/", "\\");
        
        // Open windows explorer to log file location and highlight the file. "/select" highlights the file
        Process.Start("explorer.exe", $"/select,\"{windowsPath}\"");
    }
}
