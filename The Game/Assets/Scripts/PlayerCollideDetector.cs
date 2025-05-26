using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollider : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 2f;
    public int lives = 25;

    [Header("Fall Death Settings")]
    public float fallDistanceThreshold = 7f;

    [Header("Layer Ground")]
    [SerializeField] private LayerMask groundLayer;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;

    private Vector3 startPoint;
    private Vector3? checkpoint = null;

    private bool isDying = false;
    private bool firstTime = true;
    private bool isFalling = false;
    private Vector3 previousPosition;
    private float highestPosition;

    void Awake()
    {
        playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<PlayerMovement>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startPoint = transform.position;

        previousPosition = transform.position;
    }

    void Update()
    {
        if (isDying) return;

        bool isGrounded = IsGrounded();

        if (!isGrounded)
        {
            if (transform.position.y < previousPosition.y && firstTime)
            {
                firstTime = false;
                isFalling = true;
                highestPosition = transform.position.y;
            }
            previousPosition = transform.position;
        }

        if (isGrounded && isFalling)
        {
            if (highestPosition - transform.position.y > 6)
            {
                StartCoroutine(DieAndRespawn());
            }
            isFalling = false;
            firstTime = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, playerMovement.jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying) return;

        if (collision.CompareTag("Death"))
        {
            StartCoroutine(DieAndRespawn());
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            Vector3 newCheckpoint = collision.transform.position;

            if (checkpoint == null || newCheckpoint.x > checkpoint.Value.x)
            {
                checkpoint = newCheckpoint;
                Debug.Log("Checkpoint updated: " + checkpoint);
            }
        }
    }

    private IEnumerator DieAndRespawn()
    {
        isDying = true;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;

        playerMovement.SetDeadAnimation();
        
        yield return new WaitForSeconds(respawnDelay);

        playerMovement.isDead = false;

        lives--;
        if (lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
            yield break;
        }

        Vector3 respawnPoint = checkpoint ?? startPoint;
        transform.position = respawnPoint;

        rb.simulated = true;
        col.enabled = true;
        rb.velocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetInteger("state", 0);
            animator.Play("Player Idle", 0, 0);
            animator.SetBool("IsDead", false);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isDying = false;
        // fallStartY = null;
        // wasGroundedLastFrame = true;
    }
}
