using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player, Tower, MainMenu;

    public bool inGame;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Game started!");
    }

    void OnEnable()
    { SceneManager.sceneLoaded += OnSceneLoaded; }

    void OnDisable()
    { SceneManager.sceneLoaded -= OnSceneLoaded; }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        
        if (scene.name == "Game")
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Tower = GameObject.FindGameObjectWithTag("Tower");

        }

    }

    //Maybe move the onsceneloaded stuff here if it works like that?? i dunno
    /*
    public void ThingThatDetectsARINstantiatedPrefabMarkerThing()
    {
        MainMenu = GameObject.FindGameObjectWithTag("MainMenu");

            Player = GameObject.FindGameObjectWithTag("Player");
            Tower = GameObject.FindGameObjectWithTag("Tower");

    }
    */

}

public class ThingThatDetectsARINstantiatedPrefabMarkerThing
{
    public UnityEvent theEvent;

    public void FireEvent()
    {
        theEvent?.Invoke();
    }
}
