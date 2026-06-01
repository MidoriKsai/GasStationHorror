using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsSwitcher : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> ambientSoundsToTurnOff = new();

    [SerializeField]
    private List<AudioSource> ambientSoundsToTurnOn = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOffAmbientSounds();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOnAmbientSounds();
        }
    }

    private void TurnOnAmbientSounds()
    {
        foreach (var audioSource in ambientSoundsToTurnOff)
        {
            audioSource.volume = 1f;
        }

        foreach (var audioSource in ambientSoundsToTurnOn)
        {
            audioSource.volume = 0f;
        }
    }

    private void TurnOffAmbientSounds()
    {
        foreach (var audioSource in ambientSoundsToTurnOff)
        {
            audioSource.volume = 0f;
        }

        foreach (var audioSource in ambientSoundsToTurnOn)
        {
            audioSource.volume = 0.02f;
        }
    }
}