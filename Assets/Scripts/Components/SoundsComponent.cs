using UnityEngine;

namespace Components
{
    public class SoundsComponent : MonoBehaviour
    {
        [SerializeField]
        private AudioSource twoDAudioSource;

        [SerializeField]
        private AudioSource[] threeDAudioSources;

        public AudioSource TwoDAudioSource => twoDAudioSource;

        public AudioSource[] ThreeDAudioSources => threeDAudioSources;
    }
}
