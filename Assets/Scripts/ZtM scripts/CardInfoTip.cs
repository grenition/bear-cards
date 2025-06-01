using Project.Gameplay.Battle.Model;
using Project.Gameplay.Battle.Model.Cards;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CardInfoTip : MonoBehaviour
{
    [SerializeField] private RectTransform RectTransform;

    [Space]
    [SerializeField] private Image Portrait;
    [SerializeField] private Image Ramka;
    [SerializeField] private Text Name;
    [SerializeField] private Text Obolochka;
    [SerializeField] private Image LiteralBackground;
    [SerializeField] private Text Literal;
    [SerializeField] private Text BaseInfo;

    [Space]
    [SerializeField] private LocalizedString LocalizedCost;
    [SerializeField] private LocalizedString LocalizedLevel;
    [SerializeField] private LocalizedString LocalizedDamage;
    [SerializeField] private LocalizedString LocalizedHealth;

    [Space]
    [SerializeField] private Color MetalColor;
    [SerializeField] private Color NonMetalColor;
    [SerializeField] private Color HydrogenColor;
    [SerializeField] private Color SpellColor;

    [Space]
    [SerializeField] private Color BaseRarityColor;
    [SerializeField] private Color RareRarityColor;
    [SerializeField] private Color VeryRareRarityColor;
    [SerializeField] private Color LegendaryRarityColor;

    [Space]
    [SerializeField] private RectTransform DescribtionTransform;
    [SerializeField] private Text Describtion;

    [Space]
    [SerializeField] private RectTransform PassivesContent;
    [SerializeField] private GameObject PassivePrefab;
    private GameObject[] Passives = new GameObject[0];
    private Graphic[] PassivesGraphic = new Graphic[0];

    [SerializeField] private float FadeTime;

    [Space]
    [SerializeField] private EffectInfo[] PassivesInfos;

    public Vector2 _Sizes
    {
        get
        {
            Vector2 size = new Vector2(715, RectTransform.sizeDelta.y);

            return size;
        }
    }

    public void FadePassive()
    {
        StopAllCoroutines();
        StartCoroutine(FadePasives());
    }

    public void SetInfo(CardModel card)
    {
        CardConfig config = card.Config;
        Portrait.sprite = config.VisualIcon;
        Name.text = config.LocalizedName.GetLocalizedString();
        Obolochka.text = config.ElectroFormula;
        Literal.text = config.VisualShortName;

        switch (config.Rarity)
        {
            case Project.Gameplay.Common.Datas.CardRarity.Standart:
                LiteralBackground.color = BaseRarityColor;
                break;
            case Project.Gameplay.Common.Datas.CardRarity.Rare:
                LiteralBackground.color = RareRarityColor;
                break;
            case Project.Gameplay.Common.Datas.CardRarity.VeryRare:
                LiteralBackground.color = VeryRareRarityColor;
                break;
            case Project.Gameplay.Common.Datas.CardRarity.Legendary:
                LiteralBackground.color = LegendaryRarityColor;
                break;
        }
        if(card.Key == "card_hydrogen")
        {
            Ramka.color = HydrogenColor;
        }
        else
        {
            switch (config.CardType)
            {
                case Project.Gameplay.Common.Datas.CardType.Metal:
                    Ramka.color = MetalColor;
                    break;
                case Project.Gameplay.Common.Datas.CardType.NonMetal:
                    Ramka.color = NonMetalColor;
                    break;
                case Project.Gameplay.Common.Datas.CardType.Spell:
                    Ramka.color = SpellColor;
                    break;
            }
        }

        string info = $"{LocalizedCost.GetLocalizedString()}: {config.Cost}\n{LocalizedLevel.GetLocalizedString()}: {config.Level}";

        if(config.BaseDamage == card.AttackDamage)
        {
            info += $"\n{LocalizedDamage.GetLocalizedString()}: {card.AttackDamage}";
        }
        else if(config.BaseDamage < card.AttackDamage)
        {
            info += $"\n{LocalizedDamage.GetLocalizedString()}: <color=cyan>{card.AttackDamage}</color>";
        }
        else
        {
            info += $"\n{LocalizedDamage.GetLocalizedString()}: <color=red>{card.AttackDamage}</color>";
        }

        if (config.BaseHealth == card.Health)
        {
            info += $"\n{LocalizedHealth.GetLocalizedString()}: {card.Health}";
        }
        else if (config.BaseHealth < card.Health)
        {
            info += $"\n{LocalizedHealth.GetLocalizedString()}: <color=cyan>{card.Health}</color>";
        }
        else
        {
            info += $"\n{LocalizedHealth.GetLocalizedString()}: <color=red>{card.Health}</color>";
        }

        BaseInfo.text = info;

        foreach(GameObject passive in Passives)
        {
            Destroy(passive);
        }

        float y = 0;

        Describtion.text = config.LocalizedDescribtion.GetLocalizedString();
        DescribtionTransform.sizeDelta = new Vector2(DescribtionTransform.sizeDelta.x, Describtion.preferredHeight + 35);

        y += DescribtionTransform.sizeDelta.y / 2;
        DescribtionTransform.anchoredPosition = new Vector2(0, -y);
        y += DescribtionTransform.sizeDelta.y / 2 + 15;


        Passives = new GameObject[card.Effects.Count];
        PassivesGraphic = new Graphic[0];
        for (int i = 0; i < card.Effects.Count; i++)
        {
            EffectInfo passiveInfo = GetPassive(card.Effects[i]);

            PassiveInfo passive = Instantiate(PassivePrefab, PassivesContent).GetComponent<PassiveInfo>();
            RectTransform rectTransform = passive.GetComponent<RectTransform>();

            passive.SetInfo(passiveInfo.Icon, passiveInfo.LocalizedName.GetLocalizedString(), passiveInfo.LocalizedDescribtion.GetLocalizedString());

            float height = passive._Height;

            y += height / 2f;

            rectTransform.anchoredPosition = new Vector2(0, -y);

            y += height / 2f + 15;

            Passives[i] = passive.gameObject;
            PassivesGraphic = StaticTools.ExpandMassive(PassivesGraphic, passive.gameObject.GetComponentsInChildren<Graphic>());
        }

        foreach (Graphic graphic in PassivesGraphic)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0);
        }

        RectTransform.sizeDelta = new Vector2(350, Mathf.Max(y, 600));
    }

    private EffectInfo GetPassive(EffectTypes type)
    {
        foreach (EffectInfo info in PassivesInfos)
        {
            if(info.EffectType == type)
            {
                return info;
            }
        }

        return null;
    }

    private IEnumerator FadePasives()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float alpha = 0;
        while(alpha < 1)
        {
            alpha += Time.deltaTime / FadeTime;
            foreach (Graphic graphic in PassivesGraphic)
            {
                graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
            }
            yield return waitForEndOfFrame;
        }

        alpha = 1;
        foreach(Graphic graphic in PassivesGraphic)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
        }
    }
}
