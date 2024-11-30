using UnityEngine;

namespace Project
{
    public class UIPanelAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void Hide()=> 
            _animator.SetTrigger("hide");

        private void HidePanel() => 
            gameObject.SetActive(false);
    }
}
