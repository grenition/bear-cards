using Project.Gameplay.Battle;
using Project.Gameplay.Common.Datas;
using UnityEngine;

namespace Project.UI.Battle
{
    [RequireComponent(typeof(UICardSlot))]
    public class UISingleCardSlotInitializer : MonoBehaviour
    {
        [SerializeField] private CardPosition _cardPosition;

        private UICardSlot _cardSlot;
        private void Awake()
        {
            _cardSlot = GetComponent<UICardSlot>();
            _cardSlot.Init(BattleController.Model.GetSlotAtPosition(_cardPosition));
        }
    }
}
