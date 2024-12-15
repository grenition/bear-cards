using System.Collections.Generic;
using Project.Gameplay.Battle.Model;
using UnityEngine;
using UnityEngine.UI;

public class UIEffectsImages : MonoBehaviour
{
    public List<GameObject> effectsIcons;
    private List<EffectTypes> _effects = new List<EffectTypes>();
    
    public List<EffectTypes> Effects
    {
        get => _effects;
        set
        {
            _effects = value;
            UpdateEffects();
        }
    }
    
    public void UpdateEffects()
    {
        foreach (var effect in _effects)
        {
            foreach (var eff in effectsIcons)
            {
                if(eff.name == effect.ToString()) eff.SetActive(true);
            }
        }
    }
}

