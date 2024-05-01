using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public Vector2 playerCollisionBoxSize;

    public float playerCollisionBoxXOffset;
    public float playerCollisionBoxYOffset;

    public float castDistance;
    public float bumpDelay;
    private float lastBump;
    public LayerMask playerLayer;
    private Animator anim;
    private AudioSource audioSource;

    public float bumpIntensity;

    // public AudioClip springAudio;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lastBump = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = this.getRay();
        if ((Time.time - lastBump > bumpDelay) && ray.collider != null) {
            this.PlayBump(ray.collider.transform.gameObject);
        }
    }

    private RaycastHit2D getRay()
    {
        return Physics2D.BoxCast(transform.position, playerCollisionBoxSize, 0, transform.up, castDistance, playerLayer);
    }

    public void PlayBump(GameObject item) {
        if (playerLayer != (playerLayer | (1 << item.layer))) {
            return;
        }
        PlayerMovement pmove = item.GetComponent<PlayerMovement>();
        pmove.bumpPlayer(bumpIntensity);
        lastBump = Time.time;
        audioSource.Play();
        anim.SetTrigger("bumpTrigger");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(
            transform.position + transform.up * castDistance, playerCollisionBoxSize);
    }
}
