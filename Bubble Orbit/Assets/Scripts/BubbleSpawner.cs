using UnityEngine;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;

    private Vector2 towerBoundaries;
    private float towerLowerBoundary;

    public float spawnInterval = 0.5f;
    public int maxBubbles = 25;

    List<GameObject> bubbles = new List<GameObject>();
    float timer;

    void Start()
    {
        GameObject tower = GameManager.Instance.Tower;
        if (tower == null)
        {
            Debug.LogError("Tower componnet is missing.");
            return;
        }

        towerBoundaries = tower.GetComponent<Tower>().innerAndOuterBoundary;
        towerLowerBoundary = tower.GetComponent<Tower>().lowerBoundary;

    }

    void Update()
    {
        bubbles.RemoveAll(b => b == null);

        if (bubbles.Count >= maxBubbles)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBubble();
        }
    }

    void SpawnBubble()
    {
        float dist = Random.Range(towerBoundaries.x, towerBoundaries.y);

        // FULL horizontal circle
        float yaw = Random.Range(0f, 360f);

        // vertical angle limits
        float pitch = Random.Range(towerLowerBoundary, 90f);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rot * Vector3.forward * dist;

        Vector3 pos = transform.position + offset;

        GameObject b = Instantiate(bubblePrefab, pos, Random.rotation);
        bubbles.Add(b);
    }
}
