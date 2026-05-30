using Services.Interfaces;
using UnityEngine;

namespace Services.Implementations
{
    public class SoundService : ISoundService
    {
        private AudioSource twoDAudioSource;
        private AudioSource[] threeDAudioSources;

        public void Initialize(AudioSource twoDAudioSource, AudioSource[] threeDAudioSources)
        {
            this.twoDAudioSource = twoDAudioSource;
            this.threeDAudioSources = threeDAudioSources;
        }

        public void Play2DSound(AudioClip audioClip, float volume = 1f)
        {
            twoDAudioSource.PlayOneShot(audioClip, volume);
        }

        public AudioSource Play3DSound(Vector3 position, AudioClip audioClip, float volume)
        {
            var audioSource = GetAvailableThreeDAudioSource();
            audioSource.transform.position = position;
            audioSource.PlayOneShot(audioClip, volume);
            return audioSource;
        }

        public void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        private AudioSource GetAvailableThreeDAudioSource()
        {
            foreach (var source in threeDAudioSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }

            return threeDAudioSources[0];
        }
    }
}
