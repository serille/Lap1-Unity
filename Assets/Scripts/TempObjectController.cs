using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObjectController : MonoBehaviour
{
    public float ySpeed;
    public float destroyDelay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + ySpeed * Time.deltaTime);
    }
}
