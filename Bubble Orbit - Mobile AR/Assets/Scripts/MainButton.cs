using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainButton : UIGroup
{
    Button btn; Animator anim; 
    public Shooter shooter;
    UIGroupAudioMaster audioMas;
    [SerializeField] AudioClip camshot, gameShot, shutDown, crash;
    GameManager gm; FlashScreens flashScreens;

    public bool btnMM;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);
        gm = GameManager.Instance;

        anim = GetComponent<Animator>();
        audioMas = GetComponent<UIGroupAudioMaster>();
        gm.OnAppModeChanged += AppModeChanged;

        flashScreens = msUiRef.d["Flash Screens"].obj.GetComponent<FlashScreens>();

        btnMM = true;
    }

    void OnEnable()
    {
        if (gm != null) gm.OnAppModeChanged += AppModeChanged;
    }

    void OnDisable()
    {
        if (gm != null) gm.OnAppModeChanged -= AppModeChanged;
    }

    void AppModeChanged()
    {
        switch (gm.appMode)
        {
            case GameManager.AppMode.MainMenu:
                anim.SetTrigger("Start");
                break;

            case GameManager.AppMode.Game:
                anim.SetTrigger("Game");
                break;

            case GameManager.AppMode.Exit:
                anim.SetTrigger("Exit"); audioMas.PlayClip(crash);
                break;
        }
    }

    public void ButtonClicked()
    {
        //Debug.Log("Main Button clicked");
        switch (gm.appMode)
        {
            case GameManager.AppMode.MainMenu:

                if (!btnMM) return; 
                audioMas.PlayClip(camshot);
                gm.tut.EndCurrentRect();
                if (gm.firstTime){flashScreens.PlayW(); return;}
                
                flashScreens.W_Continue();
                break;

            case GameManager.AppMode.Game:
                shooter.Shoot(); audioMas.PlayClip(gameShot);
                break;

            case GameManager.AppMode.Exit:
                btnMM = false; anim.SetTrigger("Reset");
                gm.MainMenu(); 
                break;
        }
    }

    public void ButtonClicked(string side)
    {
        if (side == "left")
        {switch (gm.appMode)
        {
            case GameManager.AppMode.MainMenu:
                
                break;

            case GameManager.AppMode.Game:
                
                break;

            case GameManager.AppMode.Exit:
                StartCoroutine(WaitForAudio(shutDown));

                break;
        }}
        else if (side == "right")
        {switch (gm.appMode)
        {
            case GameManager.AppMode.MainMenu:
                
                break;

            case GameManager.AppMode.Game:
                
                break;

            case GameManager.AppMode.Exit:
                gm.StartGame();
                break;
        }}
        else {Debug.LogWarning("ButtonClicked(side) used wrongly");}
    }

    IEnumerator WaitForAudio(AudioClip clip)
    {
        audioMas.PlayClip(clip);
        yield return new WaitForSeconds(clip.length);
        if (clip == shutDown)OnAudioEnd("quit");
    }
    void OnAudioEnd(string option)
    {
        if (option =="quit") gm.QuitApp();
    }
}
