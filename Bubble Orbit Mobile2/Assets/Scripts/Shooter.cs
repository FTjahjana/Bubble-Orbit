using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    [SerializeField]private float launchForce, lifetime;

    public void Shoot()
    {   GameObject bulletInstance = Instantiate(bulletPrefab);
        bulletPrefab.transform.rotation = Quaternion.Euler(90f, transform.rotation.eulerAngles.y, 0f);
        Destroy(bulletInstance, lifetime);
        Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();  
        rb.AddForce(transform.forward *launchForce, ForceMode.Impulse);
    }
}
