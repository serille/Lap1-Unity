using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public float ySpeed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + ySpeed * Time.deltaTime);
    }
}
