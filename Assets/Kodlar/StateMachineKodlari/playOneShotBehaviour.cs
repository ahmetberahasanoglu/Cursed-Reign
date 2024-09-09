using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;

    // Delay ses zamanlayýcýsý
    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;

    // Referans alacaðýmýz AudioManager instance
    private audiomanager audioManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // AudioManager instance'ýný bul
        if (audioManager == null)
        {
            audioManager = audiomanager.Instance;
        }

        // Ses çal
        if (playOnEnter && audioManager != null)
        {
            audioManager.PlaySFX(soundToPlay, volume);
        }

        timeSinceEntered = 0;
        hasDelayedSoundPlayed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelayedSoundPlayed && audioManager != null)
        {
            timeSinceEntered += Time.deltaTime;
            if (timeSinceEntered > playDelay)
            {
                audioManager.PlaySFX(soundToPlay, volume);
                hasDelayedSoundPlayed = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ses çal
        if (playOnExit && audioManager != null)
        {
            audioManager.PlaySFX(soundToPlay, volume);
        }
    }
}
