using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public GameObject Tower;

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
        if (scene.name == "Game")
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Tower = GameObject.FindGameObjectWithTag("Tower");

        }
    }

}
