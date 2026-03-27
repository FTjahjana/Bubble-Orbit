using UnityEngine;

public class SpawnedMenu : MonoBehaviour
{
    GameObject scanscreen_f;   // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        GameManager.Instance.TowerSpawned();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSpawn()
    {
        scanscreen_f.SetActive(false);
    }

    void OnClick()
    {
        gameObject.SetActive(false);
    }
}
