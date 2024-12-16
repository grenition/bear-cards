using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIReceiptPanel : MonoBehaviour
    {
        [SerializeField] private UIReceipt _receiptPrefab;
        [SerializeField] private Transform _receiptsRoot;

        private void Start()
        {
            _receiptsRoot.DestroyAllChildrens();

            foreach (var craft in BattleStaticData.Crafts.Values)
            {
                var receipt = Instantiate(_receiptPrefab, _receiptsRoot);
                receipt.Init(craft);
            }
        }
    }
}
