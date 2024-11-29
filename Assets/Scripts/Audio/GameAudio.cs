using UnityEngine;

namespace Project.Audio
{
    public class GameAudio : MonoBehaviour
    {
        public static AudioSource SFXSource { get; private set;} 
        public static AudioSource MusicSource { get; private set;}
        
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        
        private void Awake()
        {
            SFXSource = _sfxSource;
            MusicSource = _musicSource;
        }
    }
}
