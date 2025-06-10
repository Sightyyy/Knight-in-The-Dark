using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerCollider : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 2f;
    public int lives = 25;
    public TMP_Text livesText;

    [Header("Fall Death Settings")]
    public float fallDistanceThreshold = 7f;

    [Header("Layer Ground")]
    [SerializeField] private LayerMask groundLayer;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    AudioCollection audioCollection;
    public float fallStartCheckDelay = 0.1f;
    public float fallTimer = 0f;
    public float fallSpeedThreshold = -2f;

    private Vector3 startPoint;
    private Vector3? checkpoint = null;

    public bool isDying = false;
    private bool firstTime = true;
    private bool isFalling = false;
    private Vector3 previousPosition;
    private float highestPosition;

    void Awake()
    {
        playerMovement = GameObject.FindAnyObjectByType<PlayerMovement>().GetComponent<PlayerMovement>();
        audioCollection = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioCollection>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startPoint = transform.position;
        previousPosition = transform.position;

        UpdateLivesText();
    }

    void Update()
    {
        if (isDying) return;

        bool isGrounded = IsGrounded();
        float verticalVelocity = rb.velocity.y;

        if (!isGrounded)
        {
            if (verticalVelocity < fallSpeedThreshold)
            {
                fallTimer += Time.deltaTime;

                if (fallTimer >= fallStartCheckDelay && firstTime)
                {
                    firstTime = false;
                    isFalling = true;
                    highestPosition = transform.position.y;
                }
            }
            else
            {
                fallTimer = 0f;
            }
        }

        if (isGrounded && isFalling)
        {
            float fallDistance = highestPosition - transform.position.y;

            if (fallDistance > fallDistanceThreshold)
            {
                
                StartCoroutine(DieAndRespawn());
                audioCollection.StopSFX();
                audioCollection.StopPlayBGM();
                audioCollection.StopPlayVO();
                audioCollection.PlaySFX(audioCollection.vo9);
                audioCollection.PlaySFX(audioCollection.death2);
            }

            isFalling = false;
            firstTime = true;
            fallTimer = 0f;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center,
            col.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying) return;

        if (collision.CompareTag("Death"))
        {
            StartCoroutine(DieAndRespawn());
            audioCollection.StopSFX();
            audioCollection.StopPlayBGM();
            audioCollection.StopPlayVO();
            audioCollection.PlaySFX(audioCollection.vo8);
            audioCollection.PlaySFX(audioCollection.death);
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            Vector3 newCheckpoint = collision.transform.position;
            if (newCheckpoint != checkpoint)
            {
                checkpoint = newCheckpoint;
                audioCollection.StopSFX();
                audioCollection.StopPlayBGM();
                audioCollection.PlaySFX(audioCollection.checkpoint);
                Debug.Log("Checkpoint updated: " + checkpoint);
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator DieAndRespawn()
    {
        isDying = true;

        rb.velocity = Vector2.zero;
        rb.simulated = false;
        col.enabled = false;

        playerMovement.SetDeadAnimation();

        yield return new WaitForSeconds(respawnDelay);

        playerMovement.isDead = false;

        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            SceneManager.LoadScene("Game Over");
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

        isFalling = false;
        firstTime = true;
        fallTimer = 0f;
        highestPosition = transform.position.y;

        isDying = false;
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives.ToString();
        }
    }
}
