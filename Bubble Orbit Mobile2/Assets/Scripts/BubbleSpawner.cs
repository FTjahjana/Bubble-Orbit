using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;

    private Vector2 towerBoundaries;
    private float towerLowerBoundary;

    [Header("base settings")]
    public float spawnInterval = 0.5f;
    public int maxBubbles = 25;

    [Header("launching")]
    public float launchForce = 5f,  pushDuration = 0.5f; 

    [SerializeField] List<GameObject> bubbles = new List<GameObject>();
    float timer;    int bubbleId = 0;

    void Start()
    {
        GameObject tower = transform.parent.gameObject;
        if (tower == null)
        {
            Debug.LogError("Tower componnet is missing.");
            return;
        }

        towerBoundaries = tower.GetComponent<Tower>().innerAndOuterBoundary;
        towerLowerBoundary = tower.GetComponent<Tower>().lowerBoundary_a;

    }

    void Update()
    {
        if (!GameManager.Instance.inGame) return;
        
        bubbles.RemoveAll(b => b == null);

        if (bubbles.Count >= maxBubbles) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBubble();
        }
    }

    void SpawnBubble()
    {   GameObject b = Instantiate(bubblePrefab, transform.position, Random.rotation, transform.parent.parent);
        bubbles.Add(b);

        b.name = "Bubble" + bubbleId; bubbleId++;
        b.transform.localScale = Vector3.one * Random.Range(.7f, 1.3f);
        Rigidbody rb = b.GetComponent<Rigidbody>();

        //target bubble position
        float dist = Random.Range(towerBoundaries.x + 3, towerBoundaries.y + 3);
        // H
        float yaw = Random.Range(0f, 360f);
        // V
        float pitch = Random.Range(towerLowerBoundary, 90f);
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 bubPos = rot * Vector3.forward * dist;

        rb.AddForce(bubPos.normalized * launchForce, ForceMode.Impulse);
    }

}
