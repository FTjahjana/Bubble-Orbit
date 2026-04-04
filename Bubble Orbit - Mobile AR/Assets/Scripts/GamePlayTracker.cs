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

    GameManager gm;

    void Awake()
    {
        anim = GetComponent<Animator>(); 
        gameObject.SetActive(false);
        if (GameManager.Instance != null) gm = GameManager.Instance;
    }

    void Start()
    {
        anim.enabled = true; 
    }

    void OnEnable()
    {
        if (gm != null) gm.OnAppModeChanged += AppModeChanged;
    }

    void OnDisable()
    {
        if (gm != null) gm.OnAppModeChanged -= AppModeChanged;
    }

    public void StartTimer()
    {
        if (gm.appMode != GameManager.AppMode.Game) return;
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
        Debug.Log("timerRoutine started");

        anim.enabled = true; 
        anim.ResetTrigger("Exit"); 
        if (!gm.firstTime){
            anim.SetBool("replaying", true);
            H_anim.SetBool("replaying", true);}

        anim.Play("-Exit", 0, 0f); H_anim.Play("Hourglass - 0", 0, 0f);
        anim.Update(0f); 

        gameplaySnapshot.TransitionTo(2.0f);
        yield return new WaitForSeconds(.1f);

        if (gm.gameMode == GameManager.GameMode.Rush)
        {
            timer = rushDuration;
            soundtrack.clip = rushAud; H_anim.SetTrigger("HourglassRush");
            gm.tut.SkipIndex(10);
        }
        if (gm.gameMode == GameManager.GameMode.Endless)
        {
            timer = 0; 
            soundtrack.clip = endlessAud; H_anim.SetTrigger("HourglassLoop");
            gm.tut.SkipIndex(9);
        }
        soundtrack.Play();
        CurrentScore = 0;
        UpdateUI();

        gm.tut.SetIndex(6); gm.tut.PlayNext();
        yield return new WaitWhile(() => gm.tut.gameObject.activeSelf);

        while (true)
        {
            var mode = gm.gameMode;

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

        if (gm.gameMode == GameManager.GameMode.Rush)
        {
            timer = 0f;
            UpdateUI();
            gm.EndGame();
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
         if (gm.appMode == GameManager.AppMode.MainMenu)
        {gameObject.SetActive(false);}

         if (gm.appMode == GameManager.AppMode.MainMenu)
        {gameObject.SetActive(true);}

        if (gm.appMode == GameManager.AppMode.Exit)
        {
            anim.enabled = true; anim.SetTrigger("Exit"); 
            endsoundtrack.Play(); StartCoroutine(WaitForSoundtrackEnd());
            endSnapshot.TransitionTo(1.0f);
            
        }
    }

    public void H_AnimExit(){ H_anim.enabled = true; 
        anim.SetBool("replaying", false);
        H_anim.SetBool("replaying", false);
    H_anim.SetTrigger("Exit");}

    IEnumerator WaitForSoundtrackEnd()
    {
        yield return new WaitForSeconds(endsoundtrack.clip.length - .5f);
        msUiRef.d["Main Menu"].obj.SetActive(true);
        mainsoundtrack.Play();
        mmSnapshot.TransitionTo(1.0f);
    }

    public void PlayTut()
    {
        gm.tut.EndCurrentRect();
        gm.tut.SetIndex(13);
        gm.tut.PlayNext();
    }

}
