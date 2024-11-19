using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Cards;
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
        
        private void Start()
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
