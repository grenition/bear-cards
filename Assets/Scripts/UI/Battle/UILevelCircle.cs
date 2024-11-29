using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using Project.Gameplay.Battle;
using UnityEngine;

namespace Project.UI.Battle
{
    public class UILevelCircle : MonoBehaviour
    {
        [SerializeField] private RectTransform _electronPrefab;
        [SerializeField] private List<RectTransform> _orbits = new();
        [SerializeField] private float _spawnDelay = 0.1f;
        [SerializeField] private float _rotatingSpeedOffset = 10f;
        
        private List<RectTransform> _spawnedElectrons = new();

        private void Start()
        {
            BattleController.Model.Player.OnLevelElectronsChanged += OnLevelElectronsChanged;
        }
        private void OnDestroy()
        {
            BattleController.Model.Player.OnLevelElectronsChanged -= OnLevelElectronsChanged;
        }
        
        private async void OnLevelElectronsChanged(int delta)
        {
            if (delta <= 0)
            {
                for (int i = 0; i < -delta; i++)
                {
                    var last = _spawnedElectrons.LastOrDefault();
                    if(last == null) return;

                    Destroy(last.gameObject);
                }
                return;
            }
            
            for (int i = 0; i < delta; i++)
            {
                var startValue = BattleController.Model.Player.LevelElectrons - delta;
                var iterateValue = startValue + delta;
                var level = BattleController.Model.GetElectronLevel(iterateValue);
                var root = _orbits.GetAt(level - 1);
                if(root == null) continue;

                var electron = Instantiate(_electronPrefab, root);
                _spawnedElectrons.Add(electron);
                
                electron.anchorMin = Vector2.zero;
                electron.anchorMax = Vector2.one;
                
                electron.offsetMin = Vector2.zero;
                electron.offsetMax = Vector2.one;

                if (electron.TryGetComponent(out UIRotatingZAxis rotatingZAxis))
                    rotatingZAxis.rotationSpeed -= i * _rotatingSpeedOffset;
                
                await UniTask.WaitForSeconds(_spawnDelay);
            }            
        }
    }
}
