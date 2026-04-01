using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CollisionToBubble : MonoBehaviour
{
    [Tooltip("Dont assign to this, this fetches the bubbel audio")][SerializeField] private AudioSource audioSource;
    [SerializeField] private string requiredTag = "";

    [SerializeField] private AudioClip enterClip;
    [SerializeField] private AudioMixerSnapshot insideSnapshot, gameplaySnapshot;
    [SerializeField] private AudioClip exitClip;

    Bubble currentBubble; TowerOrbit T_Orbit;
    [Range(0f, 2f)][SerializeField]float T_newSpeedMultiplier;

     [SerializeField] private float transitionTime = 1.0f;

    bool inside = false;

    void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged += AppModeChanged;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged -= AppModeChanged;
    }
    
    void AppModeChanged()
    {
        if (GameManager.Instance.appMode == GameManager.AppMode.Game)
        {T_Orbit =  GameManager.Instance.tower.GetComponent<TowerOrbit>();}
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            inside = true; 
            currentBubble = other.GetComponent<Bubble>();
            currentBubble.insideMesh(true);

            audioSource = currentBubble.inBubble;
            audioSource.PlayOneShot(enterClip);
            insideSnapshot.TransitionTo(transitionTime);

            T_Orbit.ChangeOrbitSpeed(T_newSpeedMultiplier);

            Debug.Log($"<color=orange>Entered Bubble{other.name}</color>");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag) && inside)
        {
            inside = false; currentBubble.insideMesh(false);
            T_Orbit.ChangeOrbitSpeed(T_newSpeedMultiplier, true);
            gameplaySnapshot.TransitionTo(transitionTime);
            audioSource.PlayOneShot(exitClip);

            Debug.Log($"<color=orange>Exited Bubble{other.name}</color>");
        }
    }
}
