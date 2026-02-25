using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player, Tower, MainMenu;

    public bool inGame = false;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("Main Menu");
    }

    void Start()
    {
        MainMenu.SetActive(true);
    }

    public void StartGame()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Tower = GameObject.FindGameObjectWithTag("Tower");
    }

    public void EndGame()
    {
        Debug.Log("Game ended!");
    }


}
