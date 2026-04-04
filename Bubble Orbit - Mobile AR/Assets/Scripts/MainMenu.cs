using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : UIGroup
{
    public string chosenGameMode;

    [System.Serializable]
    public class buttons { public Button button; public bool state; }
    public List<buttons> InitButtonSettings;

    Animator anim; [SerializeField]Animator waveAnim;
    UIGroupAudioMaster audioMas;

    Button signboard, mainButton;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioMas = GetComponent<UIGroupAudioMaster>();

        foreach (var b in InitButtonSettings)
        { b.button.interactable = b.state; }

        signboard = InitButtonSettings[5].button;
        mainButton = InitButtonSettings[0].button;

        anim.SetBool("replaying", false);
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
            signboard.interactable = false;
            
            Animator mainButtonAnim = mainButton.gameObject.GetComponent<Animator>();
            mainButtonAnim.enabled = true; mainButtonAnim.SetTrigger("Start");
            anim.enabled = true; 
            if (!GameManager.Instance.firstTime) {anim.SetBool("replaying", true);}
            anim.SetTrigger("Start"); 

            audioMas.GoToSnapshot(0);
    }

    public void btnMM_true(){mainButton.GetComponent<MainButton>().btnMM = true;}
    
    void OnTowerSpawned()
    {
            mainButton.interactable = true;
            anim.SetTrigger("Focus");
    }

    public void Play()
    {
        msUiRef.d["GamePlay"].obj.SetActive(true); 
        //msUiRef.d["Flash Screens"].obj.SetActive(false);
            GameManager.Instance.SetGameMode(chosenGameMode); GameManager.Instance.StartGame();
             gameObject.SetActive(false);
        
    }

    public void Options()
    {
        Debug.Log("Options Placeholder");
    }

    public void setWaveAnim()
    {
        waveAnim.enabled = !waveAnim.isActiveAndEnabled;
    }

    public void EnableTut()
    {
        GameManager.Instance.EnableTut();
    }
    

}