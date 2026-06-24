using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;


public class Exam : MonoBehaviour
{
    // todo: finish up the question class, including looking at interactable and update
    [System.Serializable]
    public class Question
    {
        // String for question text
        public string questionString;

        // Answer Options to click
        public XRSimpleInteractable answerA;
        public XRSimpleInteractable answerB;
        public XRSimpleInteractable answerC;
        public XRSimpleInteractable answerD;

        // Text for answer choices
        public string answerAText;
        public string answerBText;
        public string answerCText;
        public string answerDText;

        public string correctAnswerText; // this is the text of the correct answer, used for grading

        private XRSimpleInteractable chosen; // update this when changing answer
        private XRSimpleInteractable correctAnswer; // correct answer is same but changes for comparison in grading

        public void Update()
        {
            //todo: when one interactable is "selected" make sure it stays that way, and "deselect" the other three 
        }

        // todo: Make UI for green checkmarks and red Xes, and make sure they show up only when grading the question
        public void GradeQuestion()
        {
            if (chosen == correctAnswer)
            {
                // mark correct answer with green arrow
                Debug.Log("Correct!");
            }
            else
            {
                // mark chosen answer with red x
                Debug.Log("Incorrect!");

            }
        }

        public string GetFormattedText(int questionNumber)
        {
            return
                $"Q{questionNumber}: {questionString}\n" +
                $"A. {answerAText}\n" +
                $"B. {answerBText}\n" +
                $"C. {answerCText}\n" +
                $"D. {answerDText}";
        }


    }

    // Set of questions we might want on the exam
    public Question[] questions;

    // This is the text that displays on the paper
    public TextMeshProUGUI paperText;

    // Keeps track of the question number we are in
    public int QuestionNumber = 0;
    
    // Testing for text on paper
    private void Start()
    {
        if (questions.Length == 0)
        {
            questions = new Question[]{

                new Question{
                    questionString = "What did the elderly woman want when she held the man’s arm?",
                    answerAText = "She was trying to avoid falling.",
                    answerBText = "She wanted help crossing the street.",
                    answerCText = "She wanted to go for a walk with him.",
                    answerDText = "She mistook him for her son.",
                    correctAnswerText = "She wanted help crossing the street."
                },
                new Question{
                    questionString = "What decision did the man make after looking at his hand?",
                    answerAText = "He wanted to play.",
                    answerBText = "He decided to go to sleep.",
                    answerCText = "He wanted to find someone he cared about.",
                    answerDText = "He decided to help people.",
                    correctAnswerText = "He decided to help people."
                },
                new Question{
                    questionString = "Why did the man pretend to be blind?",
                    answerAText = "He wanted the other man to help him.",
                    answerBText = "He was hiding from another person.",
                    answerCText = "He lost his ability to see.",
                    answerDText = "He was conducting an experiment.",
                    correctAnswerText = "He wanted the other man to help him."
                }
            };
        }
        


        // Build the full exam text (currently, will not all fit on paper, will need to either decrease font or make different pages).
        // Am leaning towards different pages, but for now just want to get the text on the paper and worry about formatting later.

        string fullText = "";

        for (int i = 0; i < questions.Length; i++)
        {
            fullText += questions[i].GetFormattedText(i + 1) + "\n";
        }

        // Set the paper text to the full exam text

        paperText.text = fullText;

        // If its null something wrong
        if (paperText == null)
        {
            Debug.LogError("Paper Text is not assigned!");
            return;
        }

    }



    void GradeExam()
    {
        foreach (var q in questions)
            q.GradeQuestion();
    }
}
