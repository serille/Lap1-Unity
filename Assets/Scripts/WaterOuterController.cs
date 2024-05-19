using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterOuterController : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject splashPrefab;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        EventManager.sfxVolumeChanged.AddListener(SfxVolumeChanged);
        audioSource.volume = GameData.sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.sfxVolumeChanged.RemoveListener(SfxVolumeChanged);
    }

    public void SfxVolumeChanged()
    {
        audioSource.volume = GameData.sfxVolume;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            audioSource.Play();
            Instantiate(splashPrefab, new Vector3(other.gameObject.transform.position.x, this.GetComponent<BoxCollider2D>().bounds.center.y + this.GetComponent<BoxCollider2D>().bounds.extents.y + this.splashPrefab.GetComponent<SpriteRenderer>().bounds.extents.y, other.gameObject.transform.position.z), Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            PlayerMovement pmove = other.gameObject.GetComponent<PlayerMovement>();
            pmove.leaveWater();
        }
    }
}
