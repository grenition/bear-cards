using NUnit.Framework;
using Project.Gameplay.Battle.Craft;
using Project.UI.Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class ProvideReceptUI : MonoBehaviour
    {
        [SerializeField] private RecipeGettedUI _recipeGettedUI;
        [SerializeField] private UIReceipt[] _uiReceipts;

        private List <CardCraftConfig> _cardCraftConfigs;

        void Start ()
        {
            _cardCraftConfigs = Resources.LoadAll<CardCraftConfig>("Gameplay/Crafts/").ToList();
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
                Debug.LogError(_cardCraftConfigs[0].name + " " + receptKey[i]);
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