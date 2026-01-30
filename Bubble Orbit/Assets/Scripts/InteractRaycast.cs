using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractRaycast : MonoBehaviour
{
    public PlayerInput playerInput;
    InputAction interactAction;
    InputAction debugAction;

    public float interactRange = 20;

    bool raydebugLogOn = false;

    float logTimer = 0f;

    float lastInteractTime = -1f;
    const float interactCooldown = 0.3f;

    public bool rayCastBlocked = false;

    private void Awake()
    {
        interactAction = playerInput.actions.FindAction("Interact");
        debugAction = playerInput.actions.FindAction("Debug");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //get forward vector
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray interactRay = new Ray(transform.position, fwd);
        RaycastHit hitData;
        Debug.DrawRay(interactRay.origin, interactRay.direction * interactRange, Color.yellow);
        //if our raycast hits something, continue
        if (Physics.Raycast(interactRay, out hitData, interactRange))
        {
            if (rayCastBlocked) {Debug.LogWarning("Raycast blocked (bool rayCastBlocked = true)."); return;}
            //checks what kind of thing we hit
            if (hitData.collider.gameObject.layer == 6)
            {
                if (raydebugLogOn) Debug.Log("Raycast has detected a " + hitData.collider.name);
                if (interactAction.WasPressedThisFrame())
                {
                    if (Time.time - lastInteractTime < interactCooldown) return;
                    lastInteractTime = Time.time;
                    
                    /*if (raydebugLogOn)*/Debug.Log("clicked " + hitData.collider.name);
                    if (raydebugLogOn)Debug.Log("Now Trying to interact with" + hitData.collider.name);
                    
                    IInteractable interactableObject =
                    hitData.collider.GetComponent<IInteractable>() ??
                    hitData.collider.GetComponentInParent<IInteractable>();

                    if (interactableObject != null) { interactableObject.Interact(); }
                }
            }
        }

        if (debugAction.WasPressedThisFrame())
        {
            raydebugLogOn = !raydebugLogOn;
            Debug.Log("Ray logs toggled!");
        }

        logTimer += Time.deltaTime;
        if (logTimer >= 1f)
    {   if (raydebugLogOn)
        {   Debug.Log("interactRay.origin: " + interactRay.origin + "interactRay.direction: " + interactRay.direction);
            Debug.Log("transform.position: " + transform.position + "transform.rotation" + transform.rotation);}
        logTimer = 0f;
    }}
}
