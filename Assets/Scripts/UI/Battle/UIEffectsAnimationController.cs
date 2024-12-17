using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle.Model;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIEffectsAnimationController : MonoBehaviour
    {
        [SerializeField ] private Animator animator;
        public void PlayEffectAnimation(EffectTypes effectType)
        {
            animator?.Play(effectType.ToString(), -1, 0);
        }
    }
}
