using System.Net.NetworkInformation;
using UnityEngine;

public class Bubble : MonoBehaviour, IInteractable
{
    [Header("floating")]
    public float floatForce = 0.4f;
    public float wanderForce = 0.2f;
    public float lifetime = 20f;

    Vector3 targetScale;

    Rigidbody rb; GamePlayTracker gamePlayTracker;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = transform.localScale;

    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Debug.Log("<color=aqua>New Bubble:</color> " + gameObject.name);

        gamePlayTracker = GameManager.Instance.gamePlayTracker;

        //growth
        transform.localScale = targetScale - (Vector3.one * .3f);
    }

    void FixedUpdate()
    {   
        //growth
        transform.localScale = Vector3.Lerp( transform.localScale, targetScale, Time.deltaTime * 2.5f);

        //drift
        rb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
        rb.AddForce(Random.insideUnitSphere * wanderForce, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision other)
    {
        //Pop();
    }

    public void Interact()
    {
        Pop();
    }

    public void Pop()
    {
        Debug.Log($"<color=red>{gameObject.name} Popped!</color>");
        gamePlayTracker.AddScore(1);
        Destroy(gameObject);
    }
}
