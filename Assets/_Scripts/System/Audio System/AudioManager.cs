using Apolos.System;
using UnityEngine;

namespace Apolos.System
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _soundEffectSource;

        public void PlaySound(AudioClip _clip)
        {
            _soundEffectSource.PlayOneShot(_clip);
        }

        public void ToggleAudio(bool isSoundOn)
        {
            _soundEffectSource.enabled = isSoundOn;
        }
    }
}


