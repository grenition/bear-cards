using TMPro;
using UnityEngine;

public class CharacterStatsTooltipDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cardTypesText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI damageTypeText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI attackPatternText;
    public TextMeshProUGUI priorityTargetText;

    private RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    [SerializeField] private float lerpFactor = 0.1f;
    [SerializeField] private float xOffset = 200f;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if(canvasGroup.alpha != 0)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 pos);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(pos.x + xOffset, pos.y), lerpFactor);
        }
    }

    public void SetStatsText(CharacterStats stats)
    {
        nameText.text = $"{stats.cardName} Stats";
        cardTypesText.text = string.Join(", ", stats.cardType);
        healthText.text = stats.health.ToString();
        damageText.text = $"{stats.damageMin} - {stats.damageMax}";
        damageTypeText.text = string.Join(", ", stats.damageType);
        rangeText.text = stats.range.ToString();
        attackPatternText.text = stats.attackPattern.ToString();
        priorityTargetText.text = stats.priorityTarget.ToString();
    }
}
