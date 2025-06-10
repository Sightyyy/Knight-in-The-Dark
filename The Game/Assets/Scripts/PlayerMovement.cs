using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] public LayerMask jumpableGround;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private float dirX = 0f;
    public bool isDead = false;

    private bool isTouchingWall = false;
    private bool isGrounded = false;
    private bool wasWalking = false;
    private AudioSource walkingAudioSource;
    AudioCollection audioCollection;

    private void Awake()
    {
        audioCollection = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioCollection>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        walkingAudioSource.clip = audioCollection.walking;
        walkingAudioSource.loop = true;
        walkingAudioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (isDead) return;

        isGrounded = IsGrounded();
        isTouchingWall = IsTouchingWall();

        dirX = Input.GetAxisRaw("Horizontal");

        if (!isGrounded && isTouchingWall)
        {
            dirX = 0f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            audioCollection.PlaySFX(audioCollection.jump);
        }

        BetterJump();
        UpdateAnimationState();
        UpdateWalkingSound();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void UpdateAnimationState()
    {
        if (isDead) return;

        if (dirX != 0f && isGrounded)
        {
            anim.SetInteger("state", 1);
            sprite.flipX = dirX < 0f;
        }
        else
        {
            anim.SetInteger("state", 0);
        }
    }

    private void UpdateWalkingSound()
    {
        bool shouldBeWalking = dirX != 0f && isGrounded && !isDead;
        
        if (shouldBeWalking && !walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        else if (!shouldBeWalking && walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }
    }

    public void SetDeadAnimation()
    {
        isDead = true;
        dirX = 0f;
        rb.velocity = Vector2.zero;
        anim.Play("Player Dead", 0, 0);
        anim.SetInteger("state", 2);
        anim.SetBool("IsDead", true);
        
        if (walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Stop();
        }
    }

    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y);
        Vector2 size = new Vector2(coll.bounds.size.x * 0.9f, 0.1f);
        return Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.05f, jumpableGround);
    }

    private bool IsTouchingWall()
    {
        Vector2 direction = sprite.flipX ? Vector2.left : Vector2.right;
        Vector2 origin = coll.bounds.center;
        Vector2 size = new Vector2(coll.bounds.size.x * 0.9f, coll.bounds.size.y * 0.9f);

        return Physics2D.BoxCast(origin, size, 0f, direction, 0.05f, jumpableGround);
    }
}