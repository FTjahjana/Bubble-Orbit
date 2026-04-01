using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotAmbience : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] ambienceClips;
    public float minDelay = 5f;
    public float maxDelay = 10f;

    private float timer = 0f;

    private void Start()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && ambienceClips.Length > 0)
        {
            audioSource.PlayOneShot(ambienceClips[Random.Range(0, ambienceClips.Length)]);
            timer = Random.Range(minDelay, maxDelay);
        }
    }
}
