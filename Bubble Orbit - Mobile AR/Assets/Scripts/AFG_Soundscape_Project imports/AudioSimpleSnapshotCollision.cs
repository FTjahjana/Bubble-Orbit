using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSimpleSnapshotCollision : MonoBehaviour
{
    [Header("Snapshot Settings")]
    [SerializeField] private AudioMixerSnapshot snapshot;
    [SerializeField] private float transitionTime = 1.0f;

    [Header("Required Tag")]
    [SerializeField] private string requiredTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            snapshot.TransitionTo(transitionTime);
        }
    }
}
