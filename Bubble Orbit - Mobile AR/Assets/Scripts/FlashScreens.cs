using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class FlashScreens : UIGroup
{

    [Header("Warnings")]
    public GameObject warnings;
    public Button w_back; public Button w_continue;

    GameManager gm; Animator anim; UIGroupAudioMaster audioMas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm = GameManager.Instance; anim = GetComponent<Animator>();
        audioMas = GetComponent<UIGroupAudioMaster>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayW(){anim.SetTrigger("W");}

    public void W_Back()
    {
        audioMas.PlayChild(0);
        gm.firstTime = false; 
        anim.SetTrigger("-W");
    }

    public void W_Continue()
    {
        audioMas.PlayChild(1);
        gm.firstTime = false; 
        anim.SetTrigger("F"); 
        msUiRef.d["Main Button"].audioMas.GoToSnapshot(0);
    }

    public void QuitApp()
    {
        anim.SetTrigger("Q");
    }

    public void PlayTut()
    {
        gm.tut.SetIndex(3);
        gm.tut.PlayNext();
    }
}
