using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject[] pages;
    public OrbitMovement orbitMovement;

    [HideInInspector]public bool inGame;
   
    void Start()
    {
        inGame = GameManager.Instance.inGame;
        if (inGame) orbitMovement.ActionsTog(false);
    }

    void OnEnable() {if (inGame) orbitMovement.ActionsTog(false);}
    void OnDisable() {if (inGame) orbitMovement.ActionsTog(true);}

    public void ShowPage(int PageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == PageIndex);
        }
    }

    public void Play()
    {
        if (!inGame) {GameManager.Instance.inGame = true;}
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