using Project.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI.Common
{
    public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private AudioClip _hoverClip;
        [SerializeField] private AudioClip _clickClip;
     
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameAudio.SFXSource.PlayOneShot(_hoverClip);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            GameAudio.SFXSource.PlayOneShot(_clickClip);
        }
    }
}
