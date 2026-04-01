using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Audio;

public class ClickToPlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnMouseDown()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clickSound);
        }
    }
}



