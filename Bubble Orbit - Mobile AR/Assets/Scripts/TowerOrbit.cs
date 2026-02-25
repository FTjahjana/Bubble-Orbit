using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Animations;

public class TowerOrbit : MonoBehaviour
{
    public Transform globalAxis;   private bool isAR;
    GameObject player;

    [Header("Input Actions")]
    public InputAction moveAction;   // WASD + QE
    public InputAction debugAction; //F12

    [Header("Movement Speeds - Automatic")]
    public bool AutomaticOrbit = true;
    public float globalOrbitSpeed = 33f; // units/sec

    [Header("Movement Speeds - Controlled")]
    public float AD_moveSpeed = 20f;   // deg/sec
    public float WS_moveSpeed = 5f;    // units/sec
    public float QE_moveSpeed = 20f;   // deg/sec

    [Header("Limits")]
    private Vector2 distFromPlayerLimits;
    public Vector2 verticalAngleLimits = new Vector2(-45f, 45f);

    [Header("Camera")]
    private float distFromPlayer;
    [SerializeField] float initialDistfromPlayer;

    void Awake()
    {
        moveAction = FindFirstObjectByType<PlayerInput>().actions.FindAction("Move");
        debugAction = FindFirstObjectByType<PlayerInput>().actions.FindAction("Debug");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isAR = GameManager.Instance.isAR;
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    void Start()
    {
        player = GameManager.Instance.Player;
        if (player == null)
        {
            Debug.LogError("Player componnet is missing.");
            return;
        }

        globalAxis = transform.parent;

        globalAxis.position = player.transform.position;
        globalAxis.rotation = Quaternion.identity;

        distFromPlayerLimits = GameManager.Instance.Tower.GetComponent<Tower>().innerAndOuterBoundary;

        distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        distFromPlayer = initialDistfromPlayer;

        transform.localPosition = Vector3.forward * distFromPlayer;
        
    }

    void Update()
    {
        if (!GameManager.Instance.inGame) return;
        
        AutoRotation();

        if (!isAR)
        {
            ControlledOrbit();
        }
    }

    void FixedUpdate()
    {
         if (debugAction.WasPressedThisFrame())
        {
            Debug.Log($"Pos:{transform.position}, Parent:{transform.parent.position}, Player: {player.transform.position}");
        }
    }

    // ---------------- ROTATION : ORBIT ----------------
    void AutoRotation()
    {
        globalAxis.Rotate(Vector3.up, globalOrbitSpeed * Time.deltaTime, Space.World);
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
            distFromPlayer -= moveInput.z * WS_moveSpeed * Time.deltaTime;
            distFromPlayer = Mathf.Clamp(distFromPlayer, distFromPlayerLimits.x, distFromPlayerLimits.y);
            transform.localPosition = Vector3.forward * distFromPlayer;
        }
    }

    #if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 48; style.alignment = TextAnchor.LowerRight;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(0, 20, Screen.width, 80), $"DistFromPlayer: ({distFromPlayer})", style
        );
    }
    #endif

}