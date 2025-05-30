using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Project.Gameplay.Battle;
using UnityEngine.Localization.Settings;

public class TipManager : MonoBehaviour
{
    [SerializeField] private RectTransform Panel;
    [SerializeField] private Camera Camera;
    [SerializeField] private Animator Animator;
    [SerializeField] private Text Text;

    [SerializeField] private Tipper Tipper;

    [SerializeField] private float ShowTime;

    [SerializeField] private RectTransform CardPanel;
    [SerializeField] private Animator CardAnimator;
    [SerializeField] private CardInfoTip CardInfoTip;

    private static TipManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public static void ShowTip(Tipper tipper, bool remove)
    {
        if(Instance == null)
        {
            return;
        }

        if (remove)
        {
            if(Instance.Tipper == tipper)
            {
                Instance.Tipper = null;

                Instance.UpdateInfo();
            }

            tipper.OnInfoChange -= Instance.UpdateText;
        }
        else
        {
            if (Instance.Tipper != tipper)
            {
                Instance.Tipper = tipper;
                tipper.OnInfoChange += Instance.UpdateText;

                Instance.UpdateInfo();
            }
        }
    }

    private void UpdateInfo()
    {
        if (Tipper == null)
        {
            Text.text = "";
            Panel.gameObject.SetActive(false);
            CardPanel.gameObject.SetActive(false);
        }
        else if(Tipper is CardTipper)
        {
            CardInfoTip.SetInfo((Tipper as CardTipper)._Card);
            Panel.gameObject.SetActive(false);
            CardPanel.gameObject.SetActive(true);

            StartCoroutine(ResizeCard());
        }
        else
        {
            Text.text = Tipper._Info;
            Panel.gameObject.SetActive(true);
            CardPanel.gameObject.SetActive(false);

            StartCoroutine(Resize());
        }
    }

    public void UpdateText()
    {
        if (Tipper == null)
        {
            Text.text = "";
        }
        else
        {
            Text.text = Tipper._Info;

            Panel.sizeDelta = new Vector2(Mathf.Clamp(Text.preferredWidth + 40, 25, 600), 80);

            Panel.sizeDelta = new Vector2(Panel.sizeDelta.x, Mathf.Min(Text.preferredHeight + 40, StaticTools.ScreenHeight));
        }
    }

    protected Vector2 CalculatePosition()
    {
        Vector2 viewPort = Camera.ScreenToViewportPoint(Input.mousePosition);

        return new Vector2(1920 * viewPort.x, StaticTools.ScreenHeight * viewPort.y);
    }

    private IEnumerator Resize()
    {
        Panel.sizeDelta = new Vector2(Mathf.Clamp(Text.preferredWidth + 40, 25, 800), 80);
        Panel.sizeDelta = new Vector2(Panel.sizeDelta.x, Mathf.Min(Text.preferredHeight + 40, StaticTools.ScreenHeight));

        yield return new WaitForSecondsRealtime(ShowTime);

        Animator.Play("Fade");

        Vector2 position = CalculatePosition() + new Vector2(Panel.sizeDelta.x / 2 + 25, -Panel.sizeDelta.y / 2 - 5);

        if (position.x + Panel.sizeDelta.x / 2 + 5 > 1920)
        {
            position.x -= Panel.sizeDelta.x + 50;
        }

        if (position.y - Panel.sizeDelta.y / 2 - 5 < 0)
        {
            position.y += Panel.sizeDelta.y + 5;

            if(position.y + Panel.sizeDelta.y / 2 > StaticTools.ScreenHeight)
            {
                position.y = Panel.sizeDelta.y/2;
            }
        }

        Panel.anchoredPosition = position;
    }
    private IEnumerator ResizeCard()
    {
        yield return new WaitForSecondsRealtime(ShowTime);

        CardAnimator.Play("Fade");

        Vector2 sizes = CardInfoTip._Sizes;
        Vector2 position = CalculatePosition() + new Vector2(175 + 25, -sizes.y / 2 - 5);

        if (position.x + 540 > 1920)
        {
            position.x -= sizes.x;
        }

        if (position.y - sizes.y / 2 - 5 < 0)
        {
            position.y += sizes.y + 5;

            if (position.y + sizes.y / 2 > StaticTools.ScreenHeight)
            {
                position.y = sizes.y / 2;
            }
        }

        CardPanel.anchoredPosition = position;
    }
}
