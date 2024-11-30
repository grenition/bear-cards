using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using Project.Gameplay.Common;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UICardSlotsSpawner : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private CardContainer _cardsContainerType;
        [SerializeField] private CardOwner _cardsOwner;

        [Header("Visual")]
        [SerializeField] private UICardSlot _cardSlotPrefab;
        
        private void Awake()
        {
            var slots = BattleController.Model.GetSlotsAtPosition(_cardsOwner, _cardsContainerType);

            transform.DestroyAllChildrens();
            foreach (var slotModel in slots)
            {
                var uiSlot = Instantiate(_cardSlotPrefab, transform);
                uiSlot.Init(slotModel);
            }
        }
    }
}
