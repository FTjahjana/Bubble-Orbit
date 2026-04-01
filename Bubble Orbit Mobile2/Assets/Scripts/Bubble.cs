using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("floating")]
    public float floatForce = 0.4f;
    public float wanderForce = 0.2f;
    public float lifetime = 20f;

    Vector3 targetScale;

    Rigidbody rb; GamePlayTracker gamePlayTracker; Animator anim; 
    [SerializeField]TextMeshPro scoreText, comboText;

    [SerializeField]public AudioSource inBubble, outBubble, popBubble;
      public AudioClip greatScore, perfectScore;

    MeshRenderer meshRenderer; [SerializeField]Material innerMat, transparentInnerMat;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = transform.localScale;
        meshRenderer= GetComponent<MeshRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
        Debug.Log("<color=aqua>New Bubble:</color> " + gameObject.name);

        gamePlayTracker = GameManager.Instance.gamePlayTracker;
        anim = GetComponent<Animator>();

        //growth
        transform.localScale = targetScale - (Vector3.one * .3f);
    }

    void FixedUpdate()
    {   
        //growth
        transform.localScale = Vector3.Lerp( transform.localScale, targetScale, Time.deltaTime * 2.5f);

        //drift
        rb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
        rb.AddForce(Random.insideUnitSphere * wanderForce, ForceMode.Acceleration);
    }

    public void insideMesh(bool state)
    {
        var mats = meshRenderer.materials;
        mats[2] = state? innerMat : transparentInnerMat;
        meshRenderer.materials = mats;
    }

    public void Pop(int score, int bubblePopCount)
    {
        Debug.Log($"<color=red>{gameObject.name} Popped!</color> (+{score})");
        gamePlayTracker.AddScore(score); scoreText.text = $"{score}";

        if (score == 50) outBubble.PlayOneShot(perfectScore);
        if (score == 20) outBubble.PlayOneShot(greatScore);
        
        bool combo = bubblePopCount>=3; string trig;
        if (combo) {comboText.text = $"Combo(x{bubblePopCount})"; trig = "ComboPop";}
        else {comboText.text = "Combo"; trig = "Pop";}
        popBubble.Play();
        anim.SetTrigger(trig);
    }

    public void Pop()
    {
        popBubble.Play(); anim.SetTrigger("Pop");
    }

    
}
