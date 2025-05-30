using Project.Gameplay.Battle.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "ZtM/Effect Info")]
public class EffectInfo : ScriptableObject
{
    public EffectTypes EffectType;

    public Sprite Icon;
    public string Name;
    public string Describtion;
}
