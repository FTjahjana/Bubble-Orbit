using UnityEngine;
using UnityEngine.UI;

public class MainButton : UIGroup
{
    Button btn; Animator anim; 
    public Shooter shooter;

    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);

        anim = this.GetComponent<Animator>();
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
        Debug.Log("Main Button clicked");
        switch (GameManager.Instance.appMode)
        {
            case GameManager.AppMode.MainMenu:
                msUiRef.d["Flash Screens"].anim.SetTrigger("C");
                break;

            case GameManager.AppMode.Game:
                shooter.Shoot();
                break;

            case GameManager.AppMode.Exit:
                // exit logic here
                break;
        }
    }
}
