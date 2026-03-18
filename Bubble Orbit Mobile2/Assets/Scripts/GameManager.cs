using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player, Tower/*, MainMenu*/;

    public bool inGame = false, isAR;
    
    [Header("XR Mobile Elements")]
    [SerializeField] List<GameObject> xrElements; 
    
    [Header("Desktop Elements")]
    [SerializeField] List<GameObject> desktopElements;

    GameTimer gameTimer;

    public enum GameMode
    {
        Rush,
        Endless
    }
    public GameMode gameMode;
    public void SetGameMode(string gamemodename)
    {
        if (gamemodename == "Rush"){gameMode = GameMode.Rush;}
        if (gamemodename == "Endless"){gameMode = GameMode.Endless;}
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //ManagePlatform();
        
    }

    void Start()
    {
        Debug.Log("Started");

/*
        if (!inGame)
        {
            MainMenu = GameObject.FindGameObjectWithTag("MainMenu");
            MainMenu.SetActive(true);
        }*/
        
        //Player = GameObject.FindGameObjectWithTag("Player");
        gameTimer = GetComponent<GameTimer>(); 
    }

    public void StartGame()
    {
        inGame = true;
        if (gameMode == GameMode.Rush) gameTimer.StartTimer();
    }

    public void EndGame()
    {
        inGame = false;
        gameTimer.StopTimer();
        Debug.Log("Game ended!");
    }

    #if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48; style.alignment = TextAnchor.UpperCenter;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(0, 20, Screen.width, 80), $"inGame: ({inGame})", style
        );
    }
    #endif


}
