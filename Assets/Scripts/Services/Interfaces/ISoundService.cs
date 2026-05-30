using UnityEngine;

namespace Services.Interfaces
{
    public interface ISoundService
    {
        void Initialize(AudioSource twoDAudioSource, AudioSource[] threeDAudioSources);

        void Play2DSound(AudioClip audioClip, float volume = 1f);

        AudioSource Play3DSound(Vector3 position, AudioClip audioClip, float volume = 1f);

        void PlaySound(AudioSource audioSource, AudioClip audioClip);
    }
}
