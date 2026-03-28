using UnityEngine;
using UnityEngine.UI;

public class ButtonStatFollowParent : MonoBehaviour
{
    private Button buttonToWatch, thisButton;
    private bool lastState;

    void Start()
    {
        buttonToWatch = transform.parent.GetComponent<Button>();
        thisButton = GetComponent<Button>();
        lastState = buttonToWatch.interactable;
    }

    void Update()
    {
        if (buttonToWatch.interactable != lastState)
        {
            lastState = buttonToWatch.interactable;
            thisButton.interactable = lastState;
        }
    }
}