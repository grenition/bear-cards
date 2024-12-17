using GreonAssets.Extensions;
using GreonAssets.UI.Extensions;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Tutorial
{
    public class UITutorial : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            _closeButton.Bind(async () =>
            {
                await gameObject.CloseWithChildrensAnimationAsync();
                Destroy(gameObject);
            }).AddTo(this);
        }
    }
}
