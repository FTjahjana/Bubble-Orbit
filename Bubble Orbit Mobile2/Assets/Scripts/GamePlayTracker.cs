using UnityEngine;
using TMPro;
using System.Collections;

public class GamePlayTracker : UIGroup
{

    [Header("Timer Settings")]
    public float rushDuration = 60f;

    [Header("Timer UI")]
    [SerializeField] private float timeLeft;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    private Coroutine timerCoroutine;

    //----------------------------------------------------------
    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI scoreText;
    public int CurrentScore { get; private set; }
    

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
        timeLeft = rushDuration;
        UpdateUI();

        while (timeLeft > 0f)
        {
            if (GameManager.Instance.gameMode != GameManager.GameMode.Rush)
            {
                yield return null;
                continue;
            }

            yield return new WaitForSeconds(1f);

            timeLeft--;
            UpdateUI();
        }

        timeLeft = 0f;
        UpdateUI();
        timerCoroutine = null;
        GameManager.Instance.appMode = GameManager.AppMode.Exit;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        timeLeftText.text = ((int)timeLeft).ToString("D2");
        scoreText.text = CurrentScore.ToString("D4");
    }
}