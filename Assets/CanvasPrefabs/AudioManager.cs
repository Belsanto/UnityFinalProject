using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider masterVolumeSlider;

    public string volumeParameter = "Volume";

    void Start()
    {
        // Initialize the slider value based on the current master volume
        masterMixer.GetFloat(volumeParameter, out float currentVolume);
        masterVolumeSlider.value = currentVolume;

        // Subscribe to the Slider's OnValueChanged event
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
    }

    void ChangeMasterVolume(float volume)
    {
        masterMixer.SetFloat(volumeParameter, volume);
    }
}
