using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayRandomSpot : MonoBehaviour
{
    [Header("References")] // Headers keep the inspector tidy
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips;

    [Header("Random Delay Settings")]
    [SerializeField] private float minTime = 5f;  // Minimum interval
    [SerializeField] private float maxTime = 10f; // Maximum interval

    private void Start()
    {
        // StartCouroutine starts the random playback
        StartCoroutine(PlayRandomClipsCoroutine());
    }

    private IEnumerator PlayRandomClipsCoroutine()
    {
        while (true)
        {
            float delay = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(delay);

            if (clips.Count > 0)
            {
                int randomIndex = Random.Range(0, clips.Count);
                audioSource.PlayOneShot(clips[randomIndex]);
            }
        }
    }
}
