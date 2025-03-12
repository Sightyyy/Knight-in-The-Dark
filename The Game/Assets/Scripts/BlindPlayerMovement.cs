using UnityEngine;

public class BlindPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public bool isMoving { get; private set; } // Untuk animasi

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Cek apakah karakter sedang bergerak atau diam
        isMoving = moveInput != 0;

        // Flip sprite jika bergerak ke kiri
        if (moveInput < 0)
            spriteRenderer.flipX = true;
        else if (moveInput > 0)
            spriteRenderer.flipX = false;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
