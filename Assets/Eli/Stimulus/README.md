# Stimulus System

---

## What Is This System?

This is a small, self-contained Unity system for triggering **animations and/or sounds** in response to user input or in-game events. It's designed to be set up mostly through the Inspector, with minimal coding required after initial setup.

There are four components you'll work with:

| Component | What It Does |
|---|---|
| `Stimulus` | Plays an animation and/or sound when triggered |
| `StimulusSequence` | Triggers a list of Stimuli one after another |
| `StimulusActionTrigger` | Lets a Stimulus or StimulusSequence respond to an Input Action (keypress, controller button, etc.) |
| `StimuliCollector` | Used by the Control Panel prefab to stop all active Stimuli/Sequences at once |

> **Important mental model:** A `Stimulus` is the basic unit — one animation, one sound, one trigger. A `StimulusSequence` is just an ordered list of Stimuli with optional pauses between them. Everything else is plumbing to connect those to inputs or UI.

---

## The Stimulus Component

### What It Actually Does

When triggered, a `Stimulus`:
1. Sets a **Boolean parameter** on an Animator to its opposite value (idle → triggered)
2. Plays a **one-shot audio clip** on an AudioSource
3. After the animation or audio finishes (whichever is longer), resets itself so it can be triggered again

**The system cannot be triggered again while it's already playing.** This is intentional — `isStimulusPlaying` blocks re-triggering until the duration has elapsed.

### Prerequisites — Read Before Setting Up

The animation side of this system has one strict requirement: **your Animator Controller must use a single Boolean parameter to drive the animation you want to trigger.** Not a Trigger, not an int — a Bool. The Stimulus flips it from its idle state to the opposite when triggered, and flips it back when resetting.

> **Why a Bool and not a Trigger?** The code reads and writes the parameter by name using `GetBool`/`SetBool`. It captures the idle state on `Start()`, then toggles to the opposite on trigger and back on reset. Triggers behave differently in the Animator state machine and won't work here.

Make sure the Animator is in its **idle/untriggered state when the scene starts.** The system captures that state as the baseline on `Start()` — if it's already in the triggered state, everything will be inverted.

### Inspector Fields

**Animation**

- **Animator** — The Animator component for the GameObject you want to animate. Can be on a different GameObject.
- **Animation Trigger Parameter Name** — The exact name (case-sensitive) of the Bool parameter in your Animator Controller.
- **Reset After Trigger** — Keep this ON unless you are manually calling `ResetStimulus()` elsewhere or your Animator resets itself. If this is off, the Stimulus will never reset automatically and cannot be re-triggered.
- **Reset After Animation Ends** — When ON (default), the system waits for the animation clip to finish before resetting. When OFF, it waits for **Manual Animation Reset Delay** instead. Note: **Reset After Trigger must still be ON** for any automatic reset to happen.
- **Manual Animation Reset Delay** — Only relevant when "Reset After Animation Ends" is OFF. A 0–120 second delay before the Animator resets.

**Audio**

- **Audio Source** — An AudioSource component (can be on any GameObject).
- **Stimulus Sound** — The audio clip to play. If you assign an AudioSource but no clip, you'll get a warning and no sound will play. Both must be assigned for audio to work.

**UI Integration** *(Only needed if using the Stimulus Button prefab)*

- **Button** — A reference to the UI Button that controls this Stimulus. If assigned, the button's text will automatically show the GameObject's name and the current trigger method.
- **Title Tag / Input Text Tag** — Tags used to find the correct TextMeshPro children of the button. Leave these as default (`StimulusTitle` / `StimulusText`) unless you have a custom button setup.

**Logging / Debug**

- **Log Activity** — Logs trigger, animation start/stop, and audio start/stop events to a file via `StimulusLogger`.
- **Print Debug Statements** — Logs more granular debug messages to the Unity Console. Useful during setup.

### How to Set One Up

1. **Add the `Stimulus` component** to a GameObject in your scene.
2. **Set up your Animator Controller** with a Bool parameter, ensure the animation state transitions on that Bool, and make sure the scene starts in the idle state.
3. **Assign the Animator** in the Inspector and type the Bool parameter name exactly into "Animation Trigger Parameter Name."
4. If you want sound, **assign an AudioSource and an AudioClip.**
5. If you want a UI button, assign one (using the Stimulus Button prefab) and make sure the button's child TextMeshPro objects are tagged correctly.
6. To trigger via a UI button or UnityEvent elsewhere, wire up **`TriggerStimulus()`** as the callback.
7. To trigger via a keystroke or controller button, add a **`StimulusActionTrigger`** component (see below).

### Triggering It

There are two ways:

- **UnityEvent / Button:** In the Inspector on a Button's `OnClick`, or any UnityEvent, call `TriggerStimulus()` on the Stimulus component.
- **Input Action:** Add a `StimulusActionTrigger` to the same GameObject (details below).

---

## The StimulusSequence Component

### What It Does

`StimulusSequence` triggers a list of `Stimulus` components **one at a time, in order.** For each step, it:
1. Triggers the Stimulus
2. Waits for that Stimulus's full duration (animation or audio, whichever is longer)
3. Waits an additional optional delay (the step's **Delay After**)
4. Moves to the next step

> **Note:** The StimulusSequence does not check whether a Stimulus finished cleanly — it just waits for the calculated duration. Make sure your animation/audio lengths are accurate.

### How to Set One Up

1. Make sure all the individual `Stimulus` components you want to sequence are already set up and working on their own GameObjects.
2. **Create a new empty GameObject** and add the `StimulusSequence` component to it.
3. Click the **+** button in the Steps list to add a step.
4. Assign a **Stimulus** reference and a **Delay After** value (in seconds, 0–120) for each step.
5. Repeat for each Stimulus in the order you want them to fire.
6. Trigger it the same way as a Stimulus — via UnityEvent calling `TriggerStimulusSequence()`, or via a `StimulusActionTrigger`.

> The StimulusSequence has its own UI Button integration fields that work identically to Stimulus — it will display the sequence name and trigger method on the button.

---

## The StimulusActionTrigger Component

### What It Does

This is a small connector component. It listens for a Unity **Input Action** (from the Input System) and calls `TriggerStimulus()` or `TriggerStimulusSequence()` when that action fires.

It **must be on the same GameObject** as either a `Stimulus` or a `StimulusSequence`. On `Start()`, it checks which one is present and hooks into it automatically. If neither is found, it logs an error.

### How to Set It Up

1. Add `StimulusActionTrigger` to the **same GameObject** as your Stimulus or StimulusSequence.
2. Assign an **Input Action Reference** in the Inspector. This is a reference to an action defined in your Input Action Asset (e.g., a keyboard key, an XR controller button).
3. That's it — the component wires itself up automatically on `Start()`.

> You can use both a UnityEvent/button **and** a StimulusActionTrigger at the same time. They don't interfere with each other.

---

## The StimuliCollector Component

This component is used **exclusively by the Control Panel prefab.** You generally won't need to add or configure it yourself.

On `Start()`, it finds every `Stimulus` and `StimulusSequence` in the scene automatically (including inactive ones). It exposes three public methods:

- **`StopAllSound()`** — Stops all audio and halts any running sequences.
- **`StopAllAnimations()`** — Resets all animators and halts any running sequences.
- **`StopAllStimuli()`** — Stops everything on every Stimulus and halts sequences.

It also enforces a **singleton pattern** — only one StimuliCollector will exist in the scene. If a second one appears (e.g., from scene loading), it destroys itself.

---

## The StimulusLogger

This runs in the background automatically when a `StimulusLogger` component is present in the scene (again, typically placed on the Control Panel prefab). You don't need to interact with it directly.

It writes timestamped log entries to a `.txt` file in `Application.persistentDataPath` whenever Stimuli are triggered, play audio, or animate. On application quit, it opens Windows Explorer to the log file location automatically (this can be toggled off in the Inspector with **Open Explorer On Application Exit**).

If you're not seeing logs, make sure a `StimulusLogger` component exists somewhere in the scene and that **Log Activity** is enabled on your Stimulus components.

---

## Common Pitfalls

**"My animation won't trigger / triggers inverted"**
Check that the Animator is in its idle state when the scene starts. The system captures the idle Bool value at runtime on `Start()`. If it's already in the triggered state, the logic will be backwards.

**"My Stimulus won't re-trigger"**
The Stimulus is locked while playing. If it seems permanently locked, check that **Reset After Trigger** is enabled, or that your Animator isn't stuck in a transition. You can call `ResetStimulus()` manually to force a reset.

**"I'm getting a warning about no Animator or AudioSource"**
A Stimulus requires at least one of the two (Animator or AudioSource+Clip) to function. If neither is present, the component disables itself.

**"No sound is playing"**
Both an AudioSource **and** an AudioClip must be assigned. One without the other won't work — check for the warning in the Console.

**"My StimulusActionTrigger isn't doing anything"**
Make sure it's on the **same GameObject** as the Stimulus or StimulusSequence, and that the Input Action Reference is assigned and the action is enabled at runtime.

---

## Quick-Reference: Trigger Methods

| What you want | How to do it |
|---|---|
| Trigger via UI Button | Wire `TriggerStimulus()` to the Button's OnClick event |
| Trigger via code | Call `stimulus.TriggerStimulus()` directly |
| Trigger via keypress/controller | Add `StimulusActionTrigger`, assign Input Action Reference |
| Stop everything | Call `StopEverything()` on the Stimulus, or use StimuliCollector |
| Trigger a sequence | Call `TriggerStimulusSequence()` on the StimulusSequence |
| Stop a sequence mid-run | Call `StopSequence()` on the StimulusSequence, or use StimuliCollector |

---
