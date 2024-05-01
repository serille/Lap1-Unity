using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public LayerMask playerLayer;

    public GameObject splashPrefab;

    public float splashY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            PlayerMovement pmove = other.gameObject.GetComponent<PlayerMovement>();
            pmove.enterWater();
            this.GetComponent<AudioSource>().Play();
            Instantiate(splashPrefab, new Vector3(other.gameObject.transform.position.x, splashY, other.gameObject.transform.position.z), Quaternion.identity);
        }
    }
    // private void OnCollisionEnter2D(Collision2D coll) {
    //     if (playerLayer == (playerLayer | (1 << coll.gameObject.layer))) {
    //         PlayerMovement pmove = coll.gameObject.GetComponent<PlayerMovement>();
    //         pmove.enterWater();
    //         ContactPoint2D cont = coll.GetContact(0);
    //         Instantiate(splashPrefab, new Vector3(cont.point.x, cont.point.y + splashYOffset, coll.gameObject.transform.position.z), Quaternion.identity);
    //     }
    // }

    private void OnTriggerExit2D(Collider2D other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            PlayerMovement pmove = other.gameObject.GetComponent<PlayerMovement>();
            pmove.leaveWater();
        }
    }
}
