using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GPTChild : UIGroup
{
    GamePlayTracker GPT;

    void Start()
    {
        GPT = transform.parent.GetComponent<GamePlayTracker>();
    }

    public void UseExit(){GameManager.Instance.EndGame();}
}