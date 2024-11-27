using Project.Gameplay.Battle;
using TMPro;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIHandElectrons : MonoBehaviour
    {
        [SerializeField] private TMP_Text _electronsText;

        private void Start()
        {
            Visualize();
            BattleController.Model.Player.OnHandElectronsChanged += OnHandElectronsChanged;
        }
        private void OnDestroy()
        {
            BattleController.Model.Player.OnHandElectronsChanged -= OnHandElectronsChanged;
        }

        private void OnHandElectronsChanged(int electrons) => Visualize();
        private void Visualize()
        {
            var electrons = BattleController.Model.Player.HandElectrons;

            _electronsText.text = $"El {electrons}";
        }
    }
}
