using UnityEngine;

public class camicon : MonoBehaviour
{
    public bool markerDetected = false;

    public GameObject markerFalseText;
    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        markerFalseText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void markerDetect(){markerDetected = true;}

    public void Clicked_PreAnim()
    {
        if (!markerDetected)
        {
            markerFalseText.SetActive(true);
            return;
        }
        anim.Play("camicon");

    }

}
