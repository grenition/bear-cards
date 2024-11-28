using System;
using Project.Gameplay.Battle;
using TMPro;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UIElectronLevels : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;

        private void Start()
        {
            Visualize();
            BattleController.Model.Player.OnLevelElectronsChanged += OnLevelElectronsChanged;
        }
        private void OnDestroy()
        {
            BattleController.Model.Player.OnLevelElectronsChanged -= OnLevelElectronsChanged;
        }

        private void OnLevelElectronsChanged(int electrons) => Visualize();
        private void Visualize()
        {
            var electrons = BattleController.Model.Player.LevelElectrons;
            var level = BattleController.Model.Player.Level;

            _levelText.text = $"{level}";
        }
    }
}
