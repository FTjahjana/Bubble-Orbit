using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.InputSystem;
using System.Collections.Generic;

public class OrbitMovement : MonoBehaviour
{
    public Transform globalAxis;   

    [Header("Input Actions")]
    public PlayerInput playerInput;
    public InputAction moveAction;   // WASD + QE
    private List<InputAction> actions = new List<InputAction>();

    [Header("Movement Speeds - Automatic")]
    public float localOrbitSpeed = 20f; // deg/sec
    public float globalOrbitSpeed = 33f; // units/sec

    [Header("Movement Speeds - Controlled")]
    public float AD_moveSpeed = 20f;   // deg/sec
    public float WS_moveSpeed = 5f;    // units/sec
    public float QE_moveSpeed = 20f;   // deg/sec

    [Header("Limits")]
    private Vector2 distFromTowerLimits;
    public Vector2 verticalAngleLimits = new Vector2(-45f, 45f);

    [Header("Look")]
    public float spinSpeed = 10f; // deg/sec

    [Header("Camera")]
    public GameObject playerCamera;

    private float distFromTower;
    private float spinInput; 

    void Awake()
    {
        moveAction = playerInput.actions.FindAction("Move");

        actions.Add(moveAction);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnEnable()
    {
        ActionsTog(true);
    }

    void OnDisable()
    {
        ActionsTog(false);
    }

    void Start()
    {
        GameObject tower = GameManager.Instance.Tower;
        if (tower == null)
        {
            Debug.LogError("Tower componnet is missing.");
            return;
        }

        globalAxis = transform.parent;

        globalAxis.position = tower.transform.position;
        globalAxis.rotation = Quaternion.identity;

        distFromTowerLimits = tower.GetComponent<Tower>().innerAndOuterBoundary;

        distFromTower = Vector3.Distance(transform.position, tower.transform.position);
        distFromTower = Mathf.Clamp(distFromTower, distFromTowerLimits.x, distFromTowerLimits.y);
    }

    void Update()
    {
        AutoRotation();

        ControlledOrbit();
        ControlledSpin();
    }

    // ---------------- ROTATION : ORBIT & SPIN ----------------
    void AutoRotation()
    {
        globalAxis.Rotate(Vector3.up, globalOrbitSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up, localOrbitSpeed * Time.deltaTime, Space.Self);
    }

    void ControlledOrbit()
    {
        Vector3 moveInput = moveAction.ReadValue<Vector3>();

        // A / D  (X)
        if (moveInput.x != 0f)
        {
            globalAxis.Rotate(Vector3.up, moveInput.x * AD_moveSpeed * Time.deltaTime, Space.World);
        }

        // Q / E  (Y)
        if (moveInput.y != 0f)
        {
            Vector3 angles = globalAxis.localEulerAngles;
            float x = angles.x > 180f ? angles.x - 360f : angles.x; 

            x += moveInput.y * QE_moveSpeed * Time.deltaTime;
            x = Mathf.Clamp(x, verticalAngleLimits.x, verticalAngleLimits.y); 

            globalAxis.localRotation = Quaternion.Euler(x, angles.y, 0f);
        }

        // W / S  (Z)
        if (moveInput.z != 0f)
        {
            distFromTower -= moveInput.z * WS_moveSpeed * Time.deltaTime;
            distFromTower = Mathf.Clamp(distFromTower, distFromTowerLimits.x, distFromTowerLimits.y);
            transform.localPosition = Vector3.forward * distFromTower;
        }
    }

    void ControlledSpin()
    {
        if (spinInput != 0f)
        {
            transform.Rotate(Vector3.up, spinInput * spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void ControlledSpin(float value)
    {
        spinInput = Mathf.Clamp(value, -1f, 1f);
    }

    // ---------------- ACTIONSTOG ----------------

    public void ActionsTog(bool state, InputAction action = null)
    {
        if (action != null)
        {
            if (state) action.Enable();
            else action.Disable();
        }
        else
        {
            foreach (InputAction a in actions)
            {
                if (state) a.Enable();
                else a.Disable();
            }
        }
    }
}