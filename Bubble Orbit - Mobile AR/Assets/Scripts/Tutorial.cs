using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    [SerializeField] int currentIndex = 0;
    public float transitionTime;
    [SerializeField]GameObject tutBg;
    [SerializeField]RectTransform textContainer;
    [SerializeField]TMP_Text textBox;

     [System.Serializable]
    public class TutNode { 
        public Transform attachedObject; 
        [TextArea(2, 4)] public string description;
        public AudioClip voiceoverClip; 
        public Vector2 targetSize;

        public Vector2 textContainerTargetPos;
        public int textContainerextend = 0;

        public float transitionTime=.8f;
        public bool instant; 
    }
    public List<TutNode> tutNodes; 

    [SerializeField]AudioSource audioSource, tfxAudioSource;
    RectTransform rectTransform; CanvasGroup canvasGroup; Camera cam;
    Vector2 targetSize; Vector2 containerDefaultSize;
    bool isCreating; 

    [SerializeField] AudioClip popIn, popOut;
    [SerializeField] Animator options; [SerializeField] TMP_Text yesbtntxt, nobtntxt;
    
    GameManager gm;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = Vector2.zero;
              
        canvasGroup = GetComponent<CanvasGroup>();
        cam = Camera.main;

        if (GameManager.Instance!= null) gm = GameManager.Instance;

        gameObject.SetActive(false);
        textContainer.gameObject.SetActive(false);

        containerDefaultSize = textContainer.sizeDelta;
    }

    void OnEnable()
    {
        isCreating = false;
        audioSource.Stop();
        canvasGroup.alpha = 0;
        textContainer.gameObject.SetActive(false);

        if (currentIndex==0)StartTut();
    }

    [ContextMenu("StartTut")]
    public void StartTut()
    {
        if (currentIndex!=0) {Debug.LogError("CurrentIndex is not 0."); return;}
        CreateRect(tutNodes[0]);
    }

    public void Continue()
    {
        if (isCreating) EndCurrentRect();
        if (currentIndex >= tutNodes.Count) {EndAndReset(); return;}
        if (tutNodes[currentIndex].instant) PlayNext();
    }

    public void PlayNext(){

        if (!gm.tutOn) return;
        
        if (currentIndex >= tutNodes.Count) {EndAndReset(); return;}
        if (isCreating) EndCurrentRect();
        CreateRect(tutNodes[currentIndex]);
    }

    public void EndAndReset()
    {
        if (!gm.tutOn) return;

        if (isCreating) EndCurrentRect();
        if (currentIndex >= tutNodes.Count) {Debug.Log("All tutNodes are played. Ending");}
        gm.tutOn = false; currentIndex = 0;
    }

    void CreateRect(TutNode tutNode)
    {
        gameObject.SetActive(true);
        
        if (!gm.tutOn){
            //Debug.LogWarning("GameManager has disabled the tutorial."); return;
        }
        if (isCreating) {
            Debug.LogWarning("currently Creating. cannot create another at the same time");
        return;}

        targetSize = tutNode.targetSize;
        rectTransform.pivot = new Vector2(0f, 1f);
        
        isCreating = true;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            RectTransformUtility.WorldToScreenPoint(null, tutNode.attachedObject.position),
            null,
            out localPos
        );

        Vector2 offset = new Vector2(-tutNode.targetSize.x / 2f, tutNode.targetSize.y / 2f);

        rectTransform.anchoredPosition = localPos + offset;
        rectTransform.sizeDelta = Vector2.zero;

        textContainer.anchoredPosition = tutNode.textContainerTargetPos;

        textContainer.sizeDelta = containerDefaultSize;
        textContainer.sizeDelta += new Vector2(0, 80*tutNode.textContainerextend);

        audioSource.mute = false;
        canvasGroup.alpha = 1;
        
        StartCoroutine(CreateRectCr(tutNode));
        
    }

    IEnumerator CreateRectCr(TutNode tutNode)
    {
        float timer = 0f;
        float TT = tutNode.transitionTime;

        while (timer < TT)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / TT);

            rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero, targetSize, t);
            yield return null;
        }

        rectTransform.sizeDelta = targetSize;
        
        yield return null;

        textBox.text = tutNode.description;
        textContainer.gameObject.SetActive(true);

        if (currentIndex==1) {
        options.enabled = true; options.SetTrigger("StarterSwap");
        yesbtntxt.text = "Continue"; nobtntxt.text = "End";
        }
        

        tfxAudioSource.PlayOneShot(popIn);

        audioSource.PlayOneShot(tutNode.voiceoverClip);
    }

    [ContextMenu("EndCurrentRect")]
    public void EndCurrentRect()
    {
        if (!gameObject.activeInHierarchy) return;
        tfxAudioSource.PlayOneShot(popOut);
        
        currentIndex++;

        isCreating = false;
        audioSource.Stop();
        canvasGroup.alpha = 0;
        textContainer.gameObject.SetActive(false);

        gameObject.SetActive(false);

    }

    public void SetIndex(int setindex)
    {
        Debug.Log("SetIndex used.");
        currentIndex = setindex;
    }

    public void SkipIndex(int skippedIndex)
    {
        tutNodes[skippedIndex] = null; 
    }

    #if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 32; style.alignment = TextAnchor.UpperCenter;
        style.normal.textColor = Color.cyan;

        GUI.Label(new Rect(0, 100, Screen.width, 80), $"tutorial {(gm.tutOn?"ON":"OFF")}", style);

    }
    #endif

    
}