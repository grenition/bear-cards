using Assets.Scripts.Map;
using Cysharp.Threading.Tasks;
using GreonAssets.Extensions;
using R3;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class CraftCardUI : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.Bind(() =>
            {
                Complited();
            }).AddTo(this);
        }

        private void Complited()
        {
            ScneneLoaderStatic.LoadSceneAsync("CraftScene");
            gameObject.SetActive(false);
        }
    }
}
