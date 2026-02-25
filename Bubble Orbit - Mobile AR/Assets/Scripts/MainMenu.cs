using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject GamePlayCanvas;
    public GameObject[] pages;
    //public OrbitMovement orbitMovement;

    [HideInInspector]bool inGame;

    public string chosenGameMode;

    Animator anim; 
   
    void Start()
    {
        inGame = GameManager.Instance.inGame;
        if (inGame) gameObject.SetActive(false);
        
        anim = GetComponent<Animator>();
    }
    
    public void ChooseGameMode(string chosen)
    {
        chosenGameMode = chosen;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    public void ShowPage(int PageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == PageIndex);
        }
    }

    public void Play()
    {
        if (!inGame) {
            GameManager.Instance.SetGameMode(chosenGameMode);
            GameManager.Instance.StartGame();
            GamePlayCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Resume()
    { 
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

}