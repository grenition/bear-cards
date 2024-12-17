using GreonAssets.Extensions;
using Project.Gameplay.Battle.Data;
using System.Linq;
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
            var activeRecept = DialoguesStatic.LoadData().Recepts;

            activeRecept.ToList().ForEach(recept =>
            {
                var receipt = Instantiate(_receiptPrefab, _receiptsRoot);
                receipt.Init(BattleStaticData.Crafts.Get(recept));
            });

        }
    }
}
