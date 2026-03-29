using UnityEngine;

public class dumbwaystodie : MonoBehaviour
{
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    public void DestroySelf()
    {
       Destroy(gameObject);
    }
}