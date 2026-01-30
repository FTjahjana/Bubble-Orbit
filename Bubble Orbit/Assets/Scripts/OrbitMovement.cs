using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class OrbitMovement : MonoBehaviour
{
    public GameObject sun;
    public Transform globalAxis;   

    [Header("Input Actions")]
    public PlayerInput playerInput;
    public InputAction horizontalMoveAction;   // WASD
    public InputAction verticalMoveAction;     // QE
    private List<InputAction> actions = new List<InputAction>();

    [Header("Movement Speeds")]
    public float AD_moveSpeed = 20f;   // deg/sec
    public float WS_moveSpeed = 5f;    // units/sec
    public float QE_moveSpeed = 20f;   // deg/sec

    [Header("Limits")]
    public Vector2 distFromSunLimits = new Vector2(5f, 25f);
    [Tooltip("(Degrees)")]
    public Vector2 verticalAngleLimits = new Vector2(-45f, 45f);

    [Header("Look")]
    public float lookSpeed = 10f; // deg/sec

    [Header("Camera")]
    public GameObject playerCamera;

    private float distFromSun;
    private float lookInput; 

    void Awake()
    {
        horizontalMoveAction = playerInput.actions.FindAction("Move");
        verticalMoveAction = playerInput.actions.FindAction("VerticalMove");

        actions.Add(horizontalMoveAction);
        actions.Add(verticalMoveAction);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnEnable()
    {
        ActionsTog(true, null, "all");
    }

    void OnDisable()
    {
        ActionsTog(false, null, "all");
    }

    void Start()
    {
        globalAxis = transform.parent;

        globalAxis.position = sun.transform.position;
        globalAxis.rotation = Quaternion.identity;

        distFromSun = Vector3.Distance(transform.position, sun.transform.position);
        distFromSun = Mathf.Clamp(distFromSun, distFromSunLimits.x, distFromSunLimits.y);
    }

    void Update()
    {
        Move();
        Look();
    }

    // ---------------- MOVEMENT ----------------

    void Move()
    {
        Vector2 moveInput = horizontalMoveAction.ReadValue<Vector2>();
        float verticalInput = verticalMoveAction.ReadValue<float>();

        // A / D 
        if (moveInput.x != 0f)
        {
            globalAxis.Rotate(Vector3.up, moveInput.x * AD_moveSpeed * Time.deltaTime, Space.World);
        }

        // Q / E 
        if (verticalInput != 0f)
        {
            Vector3 angles = globalAxis.localEulerAngles;
            float x = angles.x > 180f ? angles.x - 360f : angles.x;

            x += verticalInput * QE_moveSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, verticalAngleLimits.x, verticalAngleLimits.y);

            globalAxis.localRotation = Quaternion.Euler(x, angles.y, 0f);
        }

        // W / S 
        if (moveInput.y != 0f)
        {
            distFromSun -= moveInput.y * WS_moveSpeed * Time.deltaTime;
            distFromSun = Mathf.Clamp(distFromSun, distFromSunLimits.x, distFromSunLimits.y);
            transform.localPosition = Vector3.forward * distFromSun;
        }
    }

    // ---------------- LOOK (USE UI BUTTONS) ----------------

    void Look()
    {
        if (lookInput == 0f) return;

        transform.Rotate(Vector3.up, lookInput * lookSpeed * Time.deltaTime, Space.Self);
    }

    public void Look(float value)
    {
        lookInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void LookRelease()
    {
        lookInput = 0f;
    }

    // ---------------- ACTIONSTOG ----------------

    public void ActionsTog(bool state, InputAction action = null, string AN = null)
    {
        if (AN == "all")
        {
            foreach (InputAction a in actions)
            {
                if (state) a.Enable();
                else a.Disable();
            }
        }
        else if (action != null)
        {
            if (state) action.Enable();
            else action.Disable();
        }
    }

    // ---------------- GIZMOS ----------------

    void OnDrawGizmosSelected()
    {
        if (!sun) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sun.transform.position, distFromSunLimits.x);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sun.transform.position, distFromSunLimits.y);
    }
}
