using System.Collections.Generic;
using System.Threading.Tasks;
using GreonAssets.UI.ComponentAnimations;
using UnityEngine;

namespace GreonAssets.UI.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetActiveWithAnimation(this GameObject obj, bool activeState, bool disableAfterComplete = true)
        {
            if (activeState)
                obj.SetActive(true);
            else
                obj.CloseWithAnimation(disableAfterComplete);
        }
        
        public static void CloseWithAnimation(this GameObject obj, bool disableAfterComplete = true)
        {
            if(obj == null) return;
            if(obj.TryGetComponent(out UIOpenCloseAnimations animationComponent))
            {
                animationComponent.Close(disableAfterComplete);
                return;
            }
            obj.gameObject.SetActive(obj.gameObject.activeSelf && !disableAfterComplete);
        }
        public static async Task CloseWithAnimationAsync(this GameObject obj, bool disableAfterComplete = true)
        {
            if(obj == null) return;
            if(obj.TryGetComponent(out UIOpenCloseAnimations animationComponent))
            {
                await animationComponent.CloseAsync(disableAfterComplete);
                return;
            }
            obj.gameObject.SetActive(obj.gameObject.activeSelf && !disableAfterComplete);
        }
        
        public static void CloseWithChildrensAnimation(this GameObject obj, bool disableAfterComplete = true)
        {
            _ = CloseWithChildrensAnimationAsync(obj, disableAfterComplete);
        }
        public static async Task CloseWithChildrensAnimationAsync(this GameObject obj, bool disableAfterComplete = true)
        {
            if (obj == null) return;

            var animations = obj.GetComponentsInChildren<UIOpenCloseAnimations>();

            bool rootObjectAnimated = false;
            List<Task> closeTasks = new List<Task>();

            foreach (var animation in animations)
            {
                if (animation.gameObject == obj)
                    rootObjectAnimated = true;
                closeTasks.Add(animation.CloseAsync(animation.gameObject == obj && disableAfterComplete));
            }

            await Task.WhenAll(closeTasks);

            if (!rootObjectAnimated)
                obj.SetActive(obj.activeSelf && !disableAfterComplete);
        }
    }
}
