using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : UIGroup
{
    [HideInInspector]bool inGame;

    public string chosenGameMode;

    [System.Serializable]
    public class buttons { public Button button; public bool state; }
    public List<buttons> InitButtonSettings;

    Animator anim; [SerializeField]Animator waveAnim;
   
    void Start()
    {
        inGame = GameManager.Instance.appMode == GameManager.AppMode.Game;
        if (inGame) gameObject.SetActive(false);
        
        anim = GetComponent<Animator>();

        foreach (var b in InitButtonSettings)
        { b.button.interactable = b.state; }
    }
    
    public void ChooseGameMode(string chosen)
    {
        chosenGameMode = chosen;
    }

    void OnEnable()
    {
        GameManager.Instance.OnTowerSpawned += OnTowerSpawned;
    }

    void OnDisable()
    {
        GameManager.Instance.OnTowerSpawned -= OnTowerSpawned;
    }

    public void StartApp()
    {
        if(InitButtonSettings[5].button.gameObject.name == "signboard" &&
            InitButtonSettings[0].button.gameObject.name == "(Main Button)")
        {
            InitButtonSettings[5].button.interactable = false;
            InitButtonSettings[0].button.gameObject.GetComponent<Animator>().SetTrigger("Start");
            anim.SetTrigger("Start");
        }
        else Debug.LogWarning("Failed to run MainMenu.StartApp");
    }
    
    void OnTowerSpawned()
    {
        if(InitButtonSettings[0].button.gameObject.name == "(Main Button)")
        {
            InitButtonSettings[0].button.interactable = true;
            anim.SetTrigger("Focus");
        }
        else Debug.LogWarning("Failed to run MainMenu.OnTowerSpawned");
    }

    public void Play()
    {
        if (!inGame) { msUiRef.d["GamePlay"].obj.SetActive(true); msUiRef.d["Flash Screens"].obj.SetActive(false);
            GameManager.Instance.SetGameMode(chosenGameMode); GameManager.Instance.StartGame();
             gameObject.SetActive(false);
        }
    }

    public void Options()
    {
        Debug.Log("Options Placeholder");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        
        // add anims later
    }

    public void setWaveAnim()
    {
        waveAnim.enabled = !waveAnim.isActiveAndEnabled;
    }
    

}