using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterOuterController : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject splashPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            this.GetComponent<AudioSource>().Play();
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
