using UnityEngine;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    public float rushDuration = 60f;

    [SerializeField] private float timeLeft;
    [SerializeField] private TextMeshProUGUI timeLeftText;

    private Coroutine timerCoroutine;

    public void StartTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = null;
    }

    private IEnumerator TimerRoutine()
    {
        timeLeft = rushDuration;
        UpdateUI();

        while (timeLeft > 0f)
        {
            if (!GameManager.Instance.inGame ||
                GameManager.Instance.gameMode != GameManager.GameMode.Rush)
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
    }

    private void UpdateUI()
    {
        timeLeftText.text = timeLeft.ToString("0");
    }
}