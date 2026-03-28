using UnityEngine;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    Button btn;

    void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        
    }
}
