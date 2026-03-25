using UnityEngine;

public class pew : MonoBehaviour
{
    public GameObject pewObj;
    [SerializeField]private float launchForce, lifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void peww()
    {   GameObject pewObjSpawned = Instantiate(pewObj);
        pewObj.transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, 0f);
        Destroy(pewObjSpawned, lifetime);
        Rigidbody rb = pewObjSpawned.GetComponent<Rigidbody>();  
        rb.AddForce(transform.forward *launchForce, ForceMode.Impulse);
    }
}
