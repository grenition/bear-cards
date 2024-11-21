using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Gameplay.Battle;
using Project.Gameplay.Battle.Model.Cards;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIEmptyCardSlotsHider : MonoBehaviour
    {
        private List<UICardSlot> _slots = new();
        
        private async void Start()
        {
            _slots = GetComponentsInChildren<UICardSlot>().ToList();
            BattleController.Model.OnCardTransfered += OnCardTransfered;

            await UniTask.NextFrame();
            UpdateVisualization();
        }
        private void OnDestroy()
        {
            BattleController.Model.OnCardTransfered += OnCardTransfered;
        }

        private void OnCardTransfered(CardModel cardModel, CardPosition fromPosition, CardPosition toPosition) => UpdateVisualization();
        private void UpdateVisualization()
        {
            foreach (var slot in _slots)
            {
                var wasActive = slot.gameObject.activeSelf;
                slot.gameObject.SetActive(slot.Model.Card != null);
                if (slot.gameObject.activeSelf && !wasActive)
                    slot.transform.SetAsLastSibling();
            }            
        }
    }
}
