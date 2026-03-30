using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CollisionAudioBubble : MonoBehaviour
{
    [Tooltip("Dont assign to this, this fetches the bubbel audio")][SerializeField] private AudioSource audioSource;
    [SerializeField] private string requiredTag = "";

    [SerializeField] private AudioClip enterClip;
    [SerializeField] private AudioMixerSnapshot insideSnapshot, gameplaySnapshot;
    [SerializeField] private AudioClip exitClip;

     [SerializeField] private float transitionTime = 1.0f;

    bool inside = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            inside = true;
            audioSource = other.GetComponent<Bubble>().inBubble;
            audioSource.PlayOneShot(enterClip);
            insideSnapshot.TransitionTo(transitionTime);

            Debug.Log("<color=orange>Im in a bubbl</color>");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag) && inside)
        {
            inside = false;
            gameplaySnapshot.TransitionTo(transitionTime);
            audioSource.PlayOneShot(exitClip);
        }
    }
}
