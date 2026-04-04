using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public Tower tower;

    public event System.Action OnTowerSpawned;
    public event System.Action OnAppModeChanged;

    public enum AppMode
    { MainMenu, Game, Exit, Other }
    public AppMode appMode;

    public enum GameMode
    { Rush, Endless, N}
    public GameMode gameMode;
    public void SetGameMode(string gamemodename)
    { if (gamemodename == "Rush"){gameMode = GameMode.Rush;}
    if (gamemodename == "Endless"){gameMode = GameMode.Endless;}}

    public bool firstTime = true, tutOn;

    [Header("UIGroup refs")]
    public GamePlayTracker gamePlayTracker;
    public MainMenu mainMenu;
    public FlashScreens flashScreens;

    public Tutorial tut;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);        
    }

    void Start()
    {
        Debug.Log("App Started");
    }

    public void MainMenu()
    {
        appMode = AppMode.MainMenu; OnAppModeChanged?.Invoke();
        Debug.Log("<color=yellow>gm: MainMenu()</color>");
        mainMenu.gameObject.SetActive(true); mainMenu.StartApp();

        gamePlayTracker.gameObject.SetActive(false);
        
    }

    public void EnableTut()
    {
        tut.gameObject.SetActive(tutOn);
    }

    public void StartGame()
    {
        appMode = AppMode.Game; OnAppModeChanged?.Invoke();
        if (gameMode == GameMode.Rush || gameMode == GameMode.Endless) gamePlayTracker.StartTimer();
    }

    public void EndGame()
    {
        appMode = AppMode.Exit; OnAppModeChanged?.Invoke();
        gamePlayTracker.StopTimer(); Debug.Log("Game ended!");
    }

    public void QuitApp()
    {
        flashScreens.QuitApp();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void TowerSpawned(){OnTowerSpawned?.Invoke();}

    #if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48; style.alignment = TextAnchor.UpperCenter;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(0, 20, Screen.width, 80), $"AppMode: ({appMode})", style);

    }
    #endif


}
