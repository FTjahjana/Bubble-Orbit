using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIGroupAudioMaster : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private AudioSource mainAudioSource;
    [SerializeField] private List<AudioMixerSnapshot> snapshots;
    [SerializeField] private float transitionTime = 1.0f;
    public List<AudioSource> childAudioSources;

    private void Start()
    {
        if (mainAudioSource == null)
        {
            mainAudioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayClip(AudioClip clip){mainAudioSource.PlayOneShot(clip);}

    public void PlayChild(int childIndex)
    {
        childAudioSources[childIndex].Play();
    }

    public void GoToSnapshot(int snapshotIndex)
    {
        snapshots[snapshotIndex].TransitionTo(transitionTime);
    }

    public void SetTransitionTime(float t){transitionTime = t;}

}
