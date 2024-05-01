using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterInnerController : MonoBehaviour
{
    public LayerMask playerLayer;

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
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer))) {
            PlayerMovement pmove = other.gameObject.GetComponent<PlayerMovement>();
            pmove.surfaceWater();
        }
    }
}
