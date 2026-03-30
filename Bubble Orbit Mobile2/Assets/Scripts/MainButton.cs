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
    [SerializeField] AudioClip camshot, gameShot, shutDown;

    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);

        anim = this.GetComponent<Animator>();
        audioMas = this.GetComponent<UIGroupAudioMaster>();
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

    void AppModeChanged()
    {
        switch (GameManager.Instance.appMode)
        {
            case GameManager.AppMode.MainMenu:
                anim.SetTrigger("Start");
                break;

            case GameManager.AppMode.Game:
                anim.SetTrigger("Game");
                break;

            case GameManager.AppMode.Exit:
                anim.SetTrigger("Exit");
                break;
        }
    }

    public void ButtonClicked()
    {
        //Debug.Log("Main Button clicked");
        switch (GameManager.Instance.appMode)
        {
            case GameManager.AppMode.MainMenu:
                msUiRef.d["Flash Screens"].anim.SetTrigger("C");
                audioMas.PlayClip(camshot);
                audioMas.GoToSnapshot(0);
                break;

            case GameManager.AppMode.Game:
                shooter.Shoot(); audioMas.PlayClip(gameShot);
                break;

            case GameManager.AppMode.Exit:
                audioMas.PlayClip(shutDown);
                break;
        }
    }
}
