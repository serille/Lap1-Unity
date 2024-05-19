
using UnityEngine;
using UnityEngine.UI;

public class SettingsHandlers : MonoBehaviour
{
    public void Start()
    {
        // Initialize sliders position to the current value
        foreach(Transform child in transform)
        {
            switch (child.gameObject.name)
            {
                case "MusicVolumeSlider":
                    child.GetComponentInChildren<Slider>().value = GameData.musicVolume;
                    break;
                case "SfxVolumeSlider":
                    child.GetComponentInChildren<Slider>().value = GameData.sfxVolume;
                    break;
                case "FliesVolumeSlider":
                    child.GetComponentInChildren<Slider>().value = GameData.fliesVolume;
                    break;
            }
        }
    }
    public static void musicVolumeChanged(float volume)
    {
        GameData.musicVolume = volume;
        EventManager.musicVolumeChanged.Invoke();
    }

    public static void sfxVolumeChanged(float volume)
    {
        GameData.sfxVolume = volume;
        EventManager.sfxVolumeChanged.Invoke();
    }

    public static void fliesVolumeChanged(float volume)
    {
        GameData.fliesVolume = volume;
        EventManager.fliesVolumeChanged.Invoke();
    }
}
