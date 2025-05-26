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
    [SerializeField] private float fallMultiplier = 2.5f;     // jatuh lebih cepat
    [SerializeField] private float lowJumpMultiplier = 2f;    // lompat lebih pendek saat lepas tombol

    private float dirX = 0f;
    public bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead)
        {
            return; // Jangan update animasi lagi
        }


        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        BetterJump(); // kontrol kecepatan jatuh

        UpdateAnimationState();
    }


    private void FixedUpdate()
    {
        if (isDead) return;

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void UpdateAnimationState()
    {
        if (isDead) return;

        if (dirX != 0f)
        {
            anim.SetInteger("state", 1);
            sprite.flipX = dirX < 0f;
        }
        else
        {
            anim.SetInteger("state", 0);
        }
    }



    public void SetDeadAnimation()
    {
        isDead = true;
        dirX = 0f; // pastikan tidak bergerak

        rb.velocity = Vector2.zero;

        // Langsung paksa main animasi "Death" (pastikan nama clip-nya persis!)
        anim.Play("Player Dead", 0, 0); 

        // Kalau masih pakai parameter lain, tetap set juga (opsional)
        anim.SetInteger("state", 2);
        anim.SetBool("IsDead", true);
    }



    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 2f * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1.5f * Time.deltaTime;
        }
    }


    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(coll.bounds.center.x, coll.bounds.min.y);
        Vector2 size = new Vector2(coll.bounds.size.x * 0.9f, 0.1f);
        return Physics2D.BoxCast(origin, size, 0f, Vector2.down, 0.05f, jumpableGround);
    }

}
