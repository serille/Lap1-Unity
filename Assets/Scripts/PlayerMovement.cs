using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Security;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public String playerName;
    private Rigidbody2D rb;

    private Animator anim;

    private AudioSource audioSource;
    private Vector2 ColliderSize;

    public AudioClip jumpAudio;
    public AudioClip deathAudio;

    private float inputMovement;
    private float movement;
    private float verticalVelocity;
    private float lastY;
    private bool grounded;
    private bool slippery;
    private bool solidGround;
    private float flippedModifier = 1f;
    public float speed;

    public float movementAcceleration = 1f;
    public float jumpAmplitude;
    public float iceGrip = .2f;
    public float airGrip = .5f;
    public float solidGroundGrip = 1f;

    public LayerMask groundLayer;
    public LayerMask bumperLayer;
    public LayerMask iceLayer;
    public LayerMask playerLayer;

    public Vector2 sideCollisionBoxSize;
    public Vector2 groundCollisionBoxSize;

    public float groundCollisionBoxXOffset;
    public float groundCollisionBoxYOffset;

    public float rightCollisionBoxXOffset;
    public float rightCollisionBoxYOffset;
    public float leftCollisionBoxXOffset;
    public float leftCollisionBoxYOffset;
    public float castDistance;

    public float bumperJumpAllowDelay;
    public float bumperJumpIntensity;

    private float lastBumperJump;

    public float waterSurfaceYOffset;
    private bool onWaterSurface;

    public float defaultGravityScale;
    public float underWaterGravityScale;

    public float playerCollisionYTolerance;

    public GameObject ownPrefab;

    public float deathDelay;

    private bool dead = false;
    private float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ColliderSize = GetComponent<BoxCollider2D>().size;
        lastY = transform.position.y;
        slippery = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if (dead) {
            this.UpdateDeath();
            return;
        }
        this.UpdateMovement();
        this.UpdateAnimations();
    }

    public void UpdateDeath() {
        if (Time.time - deathDelay > deathTime) {
            GameObject.Find("SpawnPoints").GetComponent<Spawner>().SpawnPrefab(this.ownPrefab);
            Destroy(this.gameObject);
        }
    }

    public void Die() {
        audioSource.clip = deathAudio;
        audioSource.Play();
        deathTime = Time.time;
        dead = true;
        anim.SetTrigger("Dead");
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Collider2D other = col.collider;
        if (playerLayer != (playerLayer | (1 << other.gameObject.layer))) {
            return;
        }
        float selfTop = rb.transform.position.y + ColliderSize.y / 2;
        float selfBottom = rb.transform.position.y - ColliderSize.y / 2;

        float otherTop = other.gameObject.transform.position.y + other.gameObject.GetComponent<BoxCollider2D>().size.y / 2;
        float otherBottom = other.gameObject.transform.position.y - other.gameObject.GetComponent<BoxCollider2D>().size.y / 2;

        if (otherBottom >= selfTop - playerCollisionYTolerance) {
            // RIP
            UnityEngine.Debug.Log("X_X");
            this.Die();
        } else if (selfBottom >= otherTop - playerCollisionYTolerance) {
            // Score
            UnityEngine.Debug.Log("Splat!");
            rb.AddForce(new Vector2(rb.velocity.x, bumperJumpIntensity * 10));
        }
    }

    // private void CheckSplat() {
    //     RaycastHit2D ray = Physics2D.BoxCast(
    //         new Vector3(transform.position.x + (groundCollisionBoxXOffset * flippedModifier),
    //                     transform.position.y + groundCollisionBoxYOffset,
    //                     transform.position.z
    //         ),
    //         groundCollisionBoxSize, 0, -transform.up, castDistance, playerLayer);

    //     if (ray.collider == null || ray.collider.transform == this.transform) {
    //         return;
    //     }

    //     ray.collider.transform.gameObject.GetComponent<PlayerMovement>().Die();
    // }
    private void UpdateAnimations()
    {

        anim.SetBool("isRunning", inputMovement != 0);
        anim.SetBool("isGrounded", grounded);

        verticalVelocity = (transform.position.y - lastY) / Time.deltaTime;
        anim.SetFloat("verticalVelocity", verticalVelocity);
        lastY = transform.position.y;
    }

    private void UpdateMovement()
    {
        inputMovement = Input.GetAxis(playerName + " - Horizontal");

        solidGround = this.isSolidGround();
        slippery = this.isSlippery();
        grounded = solidGround || slippery;

        float grip = grounded ? (slippery ? iceGrip : solidGroundGrip) : airGrip;

        if (Input.GetButtonDown(playerName + " - Jump"))
        {
            if (grounded || onWaterSurface) {
                if (onWaterSurface) {
                    onWaterSurface = false;
                    rb.gravityScale = defaultGravityScale;
                }
                rb.AddForce(new Vector2(rb.velocity.x, jumpAmplitude * 10));
                audioSource.clip = jumpAudio;
                audioSource.Play();
            } else if (Time.time - lastBumperJump < bumperJumpAllowDelay) {
                rb.AddForce(new Vector2(rb.velocity.x, bumperJumpIntensity * 10));
            }
        }

        if (inputMovement > 0 && transform.localScale.x < 0) 
        {
            flippedModifier = 1f;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if (inputMovement < 0 && transform.localScale.x > 0)
        {
            flippedModifier = -1f;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        
        float deltaTime = Time.deltaTime * 100;
        float grip_resistance = 1 - grip * deltaTime;
        if (grip_resistance < 0) {
            grip_resistance = 0;
        }
        movement = movement * grip_resistance;
        movement += inputMovement * movementAcceleration * grip * deltaTime;

        if (movement > 0 && (flippedModifier > 0 && this.isCollideRight() || flippedModifier < 0 && this.isCollideLeft())) {
            movement = 0;
        } else if (movement < 0 && (flippedModifier > 0 && this.isCollideLeft() || flippedModifier < 0 && this.isCollideRight())) {
            movement = 0;
        }
        if (movement > speed) {
            movement = speed;
        } else if (movement < -speed) {
            movement = -speed;
        }

        rb.velocity = new Vector2(movement, rb.velocity.y);
    }

    private bool isSlippery()
    {
        if (
            Physics2D.BoxCast(
                new Vector3(transform.position.x + (groundCollisionBoxXOffset * flippedModifier),
                            transform.position.y + groundCollisionBoxYOffset,
                            transform.position.z
                ),
                groundCollisionBoxSize, 0, -transform.up, castDistance, iceLayer)
            )
        {
            return true;
        }
        return false;
    }

    private bool isSolidGround()
    {
        if (
            Physics2D.BoxCast(
                new Vector3(transform.position.x + (groundCollisionBoxXOffset * flippedModifier),
                            transform.position.y + groundCollisionBoxYOffset,
                            transform.position.z
                ),
                groundCollisionBoxSize, 0, -transform.up, castDistance, groundLayer)
            )
        {
            return true;
        }
        return false;
    }

    private bool isCollideRight() {
        if (
            Physics2D.BoxCast(
                new Vector3(transform.position.x + (rightCollisionBoxXOffset * flippedModifier),
                            transform.position.y + rightCollisionBoxYOffset,
                            transform.position.z
                ),
                sideCollisionBoxSize, 0, transform.right * flippedModifier, castDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    private bool isCollideLeft() {
        if (Physics2D.BoxCast(
                new Vector3(transform.position.x + (leftCollisionBoxXOffset * flippedModifier),
                            transform.position.y + leftCollisionBoxYOffset,
                            transform.position.z
                ), sideCollisionBoxSize, 0, -transform.right * flippedModifier, castDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    public void bumpPlayer(float intensity) {
        lastBumperJump = Time.time;
        if (rb.velocity.y < 0) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        rb.AddForce(new Vector2(rb.velocity.x, intensity * 10));
    }

    public void enterWater() {
        rb.gravityScale = underWaterGravityScale;
        rb.drag = 10;
    }

    public void leaveWater() {
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.drag = 0;
        onWaterSurface = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x + (groundCollisionBoxXOffset * flippedModifier),
                            transform.position.y + groundCollisionBoxYOffset,
                            transform.position.z
            ) - transform.up * castDistance, groundCollisionBoxSize);
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x + (rightCollisionBoxXOffset * flippedModifier),
                            transform.position.y + rightCollisionBoxYOffset,
                            transform.position.z
            ) + transform.right * castDistance * flippedModifier, sideCollisionBoxSize);
        Gizmos.DrawWireCube(
            new Vector3(transform.position.x + (leftCollisionBoxXOffset * flippedModifier),
                        transform.position.y + leftCollisionBoxYOffset,
                        transform.position.z
            ) - transform.right * castDistance * flippedModifier, sideCollisionBoxSize);
    }
}
