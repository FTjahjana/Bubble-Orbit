using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlaySoundWhenCollision : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string requiredTag = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            audioSource.mute = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            audioSource.mute = true;
        }
    }
}
