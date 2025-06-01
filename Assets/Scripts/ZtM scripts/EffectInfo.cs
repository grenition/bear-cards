using Project.Gameplay.Battle.Model;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "ZtM/Effect Info")]
public class EffectInfo : ScriptableObject
{
    public EffectTypes EffectType;

    public Sprite Icon;
    public LocalizedString LocalizedName;
    public LocalizedString LocalizedDescribtion;
}
