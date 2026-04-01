using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOneShotRandomised : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips;

    public void PlayRandomClip()
    {
        {
            // Random.Range(0, clips.Count) chooses a random number from 0 to the max clip count
            int randomIndex = Random.Range(0, clips.Count);

            // (clips[randomIndex]) will choose randomly from the list
            audioSource.PlayOneShot(clips[randomIndex]);
        }
    }
}
