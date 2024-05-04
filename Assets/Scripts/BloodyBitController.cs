using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyBitController : MonoBehaviour
{
    public Sprite[] sprites;
    public float destroyDelay;

    public float trailSpawnDelay;
    public GameObject trailPrefab;
    private float lastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprites[(int)Random.Range(0, sprites.Length)];
        // have a low probability that the object stays on permanently
        if (Random.value > .2f)
        {
            Destroy(this.gameObject, destroyDelay);
        }
        lastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawn > trailSpawnDelay)
        {
            Instantiate(trailPrefab, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            lastSpawn = Time.time;
        }
    }
}
