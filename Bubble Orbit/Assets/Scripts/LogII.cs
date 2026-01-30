using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class LogII : MonoBehaviour, IInteractable
{
    private void Awake()
    {

    }

    private void Start()
    {

    }

    void Update()
    {
 
    }

    public void Interact()
    {
        Debug.Log("<color=#B00B69>Interacted with LogII.</color>");
    }

}
