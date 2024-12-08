using GreonAssets.UI.ComponentAnimations;
using UnityEngine.UI;

namespace Project.UI.Common.Extensions
{
    public static class ButtonExtensions
    {
        public static void SetActiveWithAnimation(this Button button, bool activeState)
        {
            if(button == null) return;
            if (activeState) button.gameObject.SetActive(true);
            else button.CloseWithAnimation();
        }
        public static void CloseWithAnimation(this Button button)
        {
            if(button == null) return;
            if(button.TryGetComponent(out UIOpenCloseAnimations animationComponent))
            {
                animationComponent.Close();
                return;
            }
            button.gameObject.SetActive(false);
        }
    }
}
