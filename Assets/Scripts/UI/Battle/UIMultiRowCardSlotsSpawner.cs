using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIMultiRowCardSlotsSpawner : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private CardContainer _cardsContainerType;
        [SerializeField] private CardOwner _cardsOwner;

        [Header("Prefabs")]
        [SerializeField] private Transform _rowContainerPrefab;
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
