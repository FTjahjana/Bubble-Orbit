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

    [Header("Movement Speeds - Automatic")]
    public bool AutomaticOrbit = true;
    public float globalOrbitSpeed = 33f; // units/sec

    [Header("Limits")]
    private Vector2 distFromPlayerLimits;
    public Vector2 verticalAngleLimits = new Vector2(-45f, 45f);

    [Header("Camera")]
    private float distFromPlayer;
    [SerializeField] float initialDistfromPlayer;

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
        //if (!GameManager.Instance.inGame) return;
        
        AutoRotation();

    }

    // ---------------- ROTATION : ORBIT ----------------
    void AutoRotation()
    {
        globalAxis.Rotate(Vector3.up, globalOrbitSpeed * Time.deltaTime, Space.World);
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