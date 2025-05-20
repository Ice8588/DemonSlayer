using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum HintType
{
    Red,
    Yellow,
    None
}

public class QuadrantHint : MonoBehaviour
{
    public HintType hintType = HintType.Red;

    public float appearScale = 4.0f; // 出現時的初始大小
    public float normalScale = 2.0f; // 正常大小
    public float disappearScale = 1.2f; // 消失時的大小

    public float fadeInTime = 1.0f; // 出現階段持續時間
    public float disappearTime = 2.0f;  // 可互動並消失的階段時間

    private float timer = 0f;
    private int stage = 0; // 0: 出現, 1: 可互動並消失

    private RectTransform rectTransform;
    private UnityEngine.UI.Image image;
    private Color originalColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        originalColor = image.color;

        SetAlpha(0.3f);
        rectTransform.localScale = Vector3.one * appearScale;
    }

    void OnEnable()
    {
        timer = 0f;
        stage = 0;

        // 設定不同類型的顏色
        switch (hintType)
        {
            case HintType.Red:
                originalColor = new Color(1f, 0f, 0f); // 鮮紅色
                break;
            case HintType.Yellow:
                originalColor = new Color(1f, 1f, 0f); // 黃色
                break;
        }

        SetAlpha(0.3f);
        rectTransform.localScale = Vector3.one * appearScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (stage == 0) // 出現階段
        {
            if (timer < fadeInTime)
            {
                float t = timer / fadeInTime;
                rectTransform.localScale = Vector3.Lerp(Vector3.one * appearScale, Vector3.one * normalScale, t);
                SetAlpha(Mathf.Lerp(0.3f, 1f, t));
            }
            else
            {
                stage = 1;
                timer = 0f;
            }
        }
        else if (stage == 1) // 可互動並消失階段
        {
            if (timer < disappearTime)
            {
                float t = timer / disappearTime;
                rectTransform.localScale = Vector3.Lerp(Vector3.one * normalScale, Vector3.one * disappearScale, t);
                SetAlpha(Mathf.Lerp(1f, 0.3f, t));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void SetAlpha(float a)
    {
        Color c = originalColor;
        c.a = a;
        image.color = c;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (stage == 1)
        {
            switch (hintType)
            {
                case HintType.Red:
                    Debug.Log("紅色提示：觸發攻擊！");
                    break;
                case HintType.Yellow:
                    Debug.Log("黃色提示：觸發閃避！");
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
