using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class GamePlayTracker : UIGroup
{

    [Header("Timer Settings")]
    public float rushDuration = 60f;

    [Header("Timer UI")]
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    private Coroutine timerCoroutine; 

    [SerializeField] private Sprite[] hourglassSprites; 
    [SerializeField] private Image hourglassIcon;

    //----------------------------------------------------------
    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    public int CurrentScore { get; private set; }

    Animator anim; [SerializeField] Animator H_anim;
    [SerializeField] AudioSource soundtrack, endsoundtrack, mainsoundtrack; 
    [SerializeField] AudioMixerSnapshot gameplaySnapshot,endSnapshot, mmSnapshot;
    [SerializeField] AudioClip rushAud, endlessAud;

    void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    void Start()
    {
        anim.enabled = true; 
    }

    void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged += AppModeChanged;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged -= AppModeChanged;
    }

    public void StartTimer()
    {
        if (GameManager.Instance.appMode != GameManager.AppMode.Game) return;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }

    private IEnumerator TimerRoutine()
    {   
        
        gameplaySnapshot.TransitionTo(2.0f);
        yield return new WaitForSeconds(.1f);

        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush)
        {
            timer = rushDuration; anim.enabled = true; 
            soundtrack.clip = rushAud; H_anim.SetTrigger("HourglassRush");
        }
        if (GameManager.Instance.gameMode == GameManager.GameMode.Endless)
        {
            timer = 0; anim.enabled = true;
            soundtrack.clip = endlessAud; H_anim.SetTrigger("HourglassLoop");
        }
        soundtrack.Play();
        CurrentScore = 0;
        UpdateUI();

        while (true)
        {
            var mode = GameManager.Instance.gameMode;

            if (mode == GameManager.GameMode.Rush)
            {
                if (timer <= 0f) break; 
                yield return new WaitForSeconds(1f);
                timer--; UpdateUI();

            }
            else if (mode == GameManager.GameMode.Endless)
            {
                yield return new WaitForSeconds(1f);
                timer++; UpdateUI();
            }
            
        }

        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush)
        {
            timer = 0f;
            UpdateUI();
            GameManager.Instance.EndGame();
            H_anim.enabled = false;
            
        }

        timerCoroutine = null;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        timeLeftText.text = timer<99 ? ((int)timer).ToString("D2"):((int)timer).ToString();;
        scoreText.text = CurrentScore.ToString("D5");

    }

    public void AppModeChanged()
    {
        if (GameManager.Instance.appMode == GameManager.AppMode.Exit)
        {
            anim.enabled = true; anim.SetTrigger("Exit"); 
            endsoundtrack.Play(); StartCoroutine(WaitForSoundtrackEnd());
            endSnapshot.TransitionTo(1.0f);
            
        }
    }

    public void H_AnimExit(){ H_anim.enabled = true; H_anim.SetTrigger("Exit");}

    IEnumerator WaitForSoundtrackEnd()
    {
        yield return new WaitForSeconds(endsoundtrack.clip.length - .5f);
        mainsoundtrack.Play();
        mmSnapshot.TransitionTo(1.0f);
    }

}
