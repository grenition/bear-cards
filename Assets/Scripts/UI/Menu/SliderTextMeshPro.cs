using System.Globalization;
using GreonAssets.Extensions;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class SliderTextMeshPro : MonoBehaviour
    {
        public Slider Slider; 
        public TMP_Text Text;

        private void Start()
        {
            Text.text = Mathf.RoundToInt(Slider.value).ToString();
            Slider.onValueChanged.Bind(x => Text.text = Mathf.RoundToInt(x).ToString())
                .AddTo(this);
        }
    }
}