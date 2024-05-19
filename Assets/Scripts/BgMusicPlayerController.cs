using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BgMusicPlayerController : MonoBehaviour
{
    private AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        EventManager.musicVolumeChanged.AddListener(this.MusicVolumeChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.musicVolumeChanged.RemoveListener(this.MusicVolumeChanged);
    }

    public void MusicVolumeChanged()
    {
        musicSource.volume = GameData.musicVolume;
    }
}

