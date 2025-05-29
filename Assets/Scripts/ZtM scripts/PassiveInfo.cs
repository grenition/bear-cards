using UnityEngine;
using UnityEngine.UI;

public class PassiveInfo : MonoBehaviour
{
    [SerializeField] private RectTransform RectTransform;

    [SerializeField] private Image Icon;
    [SerializeField] private Text Name;
    [SerializeField] private Text Describtion;

    public float _Height
    {
        get
        {
            return 130 + Describtion.preferredHeight;
        }
    }

    public void SetInfo(Sprite icon, string name, string describtion)
    {
        Icon.sprite = icon;
        Name.text = name;
        Describtion.text = describtion;

        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, _Height);
    }
}
