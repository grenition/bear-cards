using UnityEngine;

namespace Project.Audio
{
    public class GameAudio : MonoBehaviour
    {
        public static float? MusicVolume { get; set; }
        public static float? SFXVolume { get; set; }
        
        public static AudioSource SFXSource { get; private set;} 
        public static AudioSource MusicSource { get; private set;}
        
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _musicSource;
        
        private void Awake()
        {
            if (MusicVolume.HasValue)
            {
                _musicSource.volume = MusicVolume.Value;
            }

            if (SFXVolume.HasValue)
            {
                _sfxSource.volume = SFXVolume.Value;
            }
            
            SFXSource = _sfxSource;
            MusicSource = _musicSource;
        }
        
        public static void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            MusicSource.volume = volume;
        }
        
        public static void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            SFXSource.volume = volume;
        }
    }
}
