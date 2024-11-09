using SinuousProductions;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterStatsTooltipDisplay tooltip;
    public float fadeTime = 0.1f;

    void Awake()
    {
        tooltip = FindObjectOfType<CharacterStatsTooltipDisplay>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.SetStatsText(GetComponent<CharacterStats>());
            StartCoroutine(Utility.FadeIn(tooltip.canvasGroup, 1.0f, fadeTime));
        }
        else tooltip = FindObjectOfType<CharacterStatsTooltipDisplay>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            StartCoroutine(Utility.FadeOut(tooltip.canvasGroup, 0.0f, fadeTime));
        }
    }
}
