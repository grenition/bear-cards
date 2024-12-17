using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UISettingsPause : UISettingsBase
    {
        [SerializeField] private Button _backButton;
        protected override void Start()
        {
            _backButton.Bind(() =>
            {
                gameObject.CloseWithAnimation();
            }).AddTo(this);
            
            base.Start();
        }
    }
}
