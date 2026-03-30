using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TowerOrbit : MonoBehaviour
{
    public Transform globalAxis;   private bool isAR;
    GameObject player;

    [Header("Movement")]
    public bool AutomaticOrbit = true;
    public float globalOrbitSpeed = 33f; // units/sec
    Animator anim; 

    [Header("Limits")]
    private Vector2 distFromPlayerLimits;
    public Vector2 verticalAngleLimits = new Vector2(-45f, 45f);

    [Header("Camera")]
    private float distFromPlayer;
    [SerializeField] float initialDistfromPlayer;

    void OnEnable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged += AppModeChanged;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnAppModeChanged -= AppModeChanged;
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

        distFromPlayerLimits = GameManager.Instance.tower.innerAndOuterBoundary;

        distFromPlayer = initialDistfromPlayer; transform.localPosition = Vector3.forward * distFromPlayer;

        anim = GetComponent<Animator>(); anim.enabled = false;
        AppModeChanged();
    
    }

    void Update()
    {
        distFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (GameManager.Instance.appMode == GameManager.AppMode.MainMenu) AutoRotation(true);
        if (GameManager.Instance.appMode == GameManager.AppMode.Game) AutoRotation();

    }

    void AppModeChanged()
    {
        if (GameManager.Instance.appMode == GameManager.AppMode.Game)
        {
            anim.enabled = false;
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.position = player.transform.position + Camera.main.transform.forward * distFromPlayer;
        }

        if (GameManager.Instance.appMode == GameManager.AppMode.MainMenu)
        {
            anim.enabled = false;
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * initialDistfromPlayer;
        }

        if (GameManager.Instance.appMode == GameManager.AppMode.Exit)
        {   
            anim.enabled = true;
            anim.SetTrigger("Exit");
        }
    }

    // ---------------- ROTATION : ORBIT ----------------
    void AutoRotation(bool local = false)
    {
        Transform rotAxis = local ? transform : globalAxis;
        rotAxis.Rotate(Vector3.up, globalOrbitSpeed * Time.deltaTime, Space.World);
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