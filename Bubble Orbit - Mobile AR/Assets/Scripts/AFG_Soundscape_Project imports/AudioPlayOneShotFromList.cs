using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayOneShotFromList : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clip;

    public void PlayClip(int i)
    {
        audioSource.PlayOneShot(clip[i]);
    }
}
