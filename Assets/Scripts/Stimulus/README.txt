HOW TO USE THE STIMULUS COMPONENT:
A "Stimulus" is a component that, when triggered, plays an animation and/or a sound. A stimulus can be triggered via a Unity Event (buttons, UI, in-game events, etc.) using the "TriggerStimulus()" callback, or via an Input Action Event (keystroke, controller action, etc.) by adding the "Stimulus Action Trigger" component to the same GameObject and selecting an Input Action Reference. 

The fields of the Stimulus component have tooltips in the Inspector when hovered. 

Also, if there is any confusion in the use of the Stimulus component, comprehensive debug logs will assist their use.

1. Add the Stimulus component to the GameObject.
2. If an animation is to be triggered, ensure the animation controller for the desired GameObject is set up to use a single Boolean parameter to trigger the desired animation. Type the name of this parameter into the "Animation Trigger Parameter Name" field in the Inspector.
3. Ensure that when the Scene is run, the triggering parameter for the animation is in its idle, untriggered state.
4. Assign the animator for the desired GameObject, the Audio Source if needed, and an Audio Clip (mp3, etc.) if sound is included as part of this stimulus. 
5. The Stimulus component has a public function called "TriggerStimulus()" that can be called as a UnityEvent or a System.Action, which allows it to be called by buttons or other interactions, within XR or the UI toolkit. 
6. To make the Stimulus triggered by a specific input, such as a keystroke or XR Controller A-button, add the "Stimulus Action Trigger" component to the GameObject and assign the appropriate Input Action Reference for the keystroke or controller input. Now, the Stimulus can be triggered the same as before OR with the selected Input Action. 

About resetting the animation: the Stimulus component will reset the boolean parameter that triggers the animation by default. Disabling the "Reset After Animation Ends" flag will allow for a manually-determined time (0s-120s) before resetting the animator, but "Reset After Trigger" must remain enabled for the animator to be reset automatically. If the "Reset After Trigger" flag is disabled, it is expected that the animation controller automatically resets the triggering parameter OR the public "ResetStimulus()" function is be called elsewhere for the Stimulus. The Stimulus cannot be triggered again until the animation parameter is in its original state.

HOW TO USE THE STIMULUSSEQUENCE COMPONENT:
A "Stimulus Sequence" is a component that, when triggered, will trigger the Stimulus components in its list. For each Stimulus, the process is this: wait for the defined pre-trigger delay (0s-120s, default 0s), trigger the Stimulus, then wait for the defined post-trigger delay (0s-120s, default 0s). It will do this sequentially for each Stimulus in its list. A Stimulus Sequence can also be triggered by an Input Action Event in the same way a normal Stimulus can - by adding the "Stimulus Action Trigger" component to the same GameObject as the Stimulus Sequence and selecting an Input Action Reference.

1. Ensure you have the Stimulus components set up on the GameObjects that are intended to be stimuli, and add the StimulusSequence component to a new empty GameObject. 
2. Click the plus icon on the list to create a new step. 
3. Set the step's Stimulus reference to the first Stimulus you wish to be triggered, as well as the delay (in seconds) to occur before and after the trigger.
4. Repeat steps 2 and 3 for each stimulus you wish to play in sequence.
5. To make the Stimulus Sequence triggered by a specific input, such as a keystroke or XR Controller A-button, add the "Stimulus Action Trigger" component to the GameObject and assign the appropriate Input Action Reference for the keystroke or controller input. Now, the Stimulus Sequence can be triggered the same as before OR with the selected Input Action.
