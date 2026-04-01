using UnityEngine;

public class RectStretchAnim : MonoBehaviour
{
    public float transitionTime = 3f;

    private RectTransform rectTransform;
    private Vector2 targetSize;
    private float timer;
    private bool isAnimating;

    [SerializeField]Vector2 newsize;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        //set pivot to 0, 1 on comp
        rectTransform.sizeDelta = Vector2.zero;
    }

    public void CreateRect()
    {
        targetSize = newsize;
        timer = 0f;
        isAnimating = true;

        rectTransform.sizeDelta = Vector2.zero;
    }

    void Update()
    {
        if (!isAnimating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / transitionTime);

        rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero, targetSize, t);

        if (t >= 1f)
        {
            isAnimating = false;
        }
    }
}