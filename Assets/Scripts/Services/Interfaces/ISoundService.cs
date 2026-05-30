using UnityEngine;

namespace Services.Interfaces
{
    public interface ISoundService
    {
        void Initialize(AudioSource twoDAudioSource, AudioSource[] threeDAudioSources);

        void Play2DSound(AudioClip audioClip);

        void Play3DSound(Vector3 position, AudioClip audioClip);

        void PlaySound(AudioSource audioSource, AudioClip audioClip);
    }
}
