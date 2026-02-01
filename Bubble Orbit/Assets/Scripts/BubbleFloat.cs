using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    public float floatForce = 0.4f;
    public float wanderForce = 0.2f;
    public float lifetime = 8f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
        rb.AddForce(Random.insideUnitSphere * wanderForce, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision other)
    {
        Pop();
    }

    public void Pop()
    {
        Destroy(gameObject);
    }
}
