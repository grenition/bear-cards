using GreonAssets.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Menu
{
    public class UISettings : UISettingsBase
    {
        [SerializeField] private UIMenu _menu;
        [SerializeField] private Button _backButton;

        protected override void Start()
        {
            _backButton.Bind(_menu.BackToMain)
                .AddTo(this);

            base.Start();
        }
    }
}
