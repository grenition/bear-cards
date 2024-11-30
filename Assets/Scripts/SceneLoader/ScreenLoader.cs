using UnityEngine;

namespace Assets.Scripts.Map
{
    public class ScreenLoader : MonoBehaviour
    {
        public bool ScreenHide { get; private set; }
        public bool ScreenShow { get; private set; }

        [SerializeField] private Animator _animator;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void HideScreen() =>
            _animator.SetTrigger("hide");

        public void HideState() =>
            ScreenHide = true;

        public void ShowState() =>
            ScreenShow = true;

        private void OnHideScreen() =>
            Destroy(gameObject);
    }
}
