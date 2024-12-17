using NUnit.Framework;
using Project.Gameplay.Battle.Craft;
using Project.UI.Battle;
using System.Collections.Generic;
using System.Linq;
using Project.Gameplay.Battle.Data;
using UnityEngine;

namespace Project
{
    public class ProvideReceptUI : MonoBehaviour
    {
        [SerializeField] private RecipeGettedUI _recipeGettedUI;
        [SerializeField] private UIReceipt[] _uiReceipts;

        private List<CardCraftConfig> _cardCraftConfigs;

        void Start ()
        {
            _cardCraftConfigs = BattleStaticData.Crafts.Values.ToList();
            _recipeGettedUI.OnReceptUpdate += UpdateUIRecepts;
        }

        private void UpdateUIRecepts(string[] receptKey)
        {
            for (int i = 0; i < _uiReceipts.Length; i++)
            {
                _uiReceipts[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < receptKey.Length; i++)
            {
                var config = _cardCraftConfigs.Find(config => config.name == receptKey[i]);
                if(config != null)
                {
                    _uiReceipts[i].gameObject.SetActive(true);
                    _uiReceipts[i].Init(config);
                }
            }
        }

        private void OnDisable()
        {
            _recipeGettedUI.OnReceptUpdate -= UpdateUIRecepts;
        }
    }
}
