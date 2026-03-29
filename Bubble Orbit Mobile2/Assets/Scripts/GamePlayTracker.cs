using UnityEngine;
using TMPro; using UnityEngine.UI;
using System.Collections;

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

    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>(); anim.enabled = true; 
        GameManager.Instance.OnAppModeChanged += AppModeChanged;
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
        yield return new WaitForSeconds(.5f);

        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush) 
        {timer = rushDuration; anim.enabled = false;}
        if (GameManager.Instance.gameMode == GameManager.GameMode.Endless) 
        {timer = 0; anim.enabled = true; anim.SetTrigger("HourglassLoop");}

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

        if (((int)timer)%((int)rushDuration/4)==0) hourglassIcon.sprite = hourglassSprites[((int)timer)/((int)rushDuration/4)-1];

    }

    public void AppModeChanged()
    {
        if (GameManager.Instance.appMode == GameManager.AppMode.Paused){paused = true;}
        if (GameManager.Instance.appMode == GameManager.AppMode.Game){ paused = false;
        if (GameManager.Instance.gameMode == GameManager.GameMode.Rush) anim.enabled = false;
        if (GameManager.Instance.gameMode == GameManager.GameMode.Endless) anim.enabled = true; 
        }
    }
}