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

    private Coroutine timerCoroutine; private bool paused = false;

    [SerializeField] private Sprite[] hourglassSprites; 
    [SerializeField] private Image hourglassIcon;

    //----------------------------------------------------------
    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    public int CurrentScore { get; private set; }

    Animator anim; [SerializeField] Animator H_anim;
    [SerializeField] AudioSource soundtrack; 
    [SerializeField] AudioMixerSnapshot gameplaySnapshot;
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
        paused = false;

        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }

    private IEnumerator TimerRoutine()
    {   
        
        gameplaySnapshot.TransitionTo(1.0f);
        yield return new WaitForSeconds(.5f);

        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush)
        {
            timer = rushDuration; anim.enabled = true; 
            soundtrack.clip = rushAud; H_anim.SetTrigger("HourglassRush");
        }
        if (GameManager.Instance.gameMode == GameManager.GameMode.Endless)
        {
            timer = 0; anim.enabled = true;
            soundtrack.clip = rushAud; H_anim.SetTrigger("HourglassLoop");
        }
        soundtrack.Play();
        CurrentScore = 0;
        UpdateUI();

        while (true)
        {
            if (!paused){var mode = GameManager.Instance.gameMode;

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
            else
            {
                yield return null; 
            }}
            
        }

        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush)
        {
            timer = 0f;
            UpdateUI();
            GameManager.Instance.EndGame();
            H_anim.enabled = false;
            anim.enabled = true; anim.SetTrigger("Exit");
            
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
        if (GameManager.Instance.appMode == GameManager.AppMode.Paused){paused = true;}
        if (GameManager.Instance.appMode == GameManager.AppMode.Game){ paused = false;
        //if (GameManager.Instance.gameMode == GameManager.GameMode.Rush) ;
        //if (GameManager.Instance.gameMode == GameManager.GameMode.Endless) ; 
        }
    }
}