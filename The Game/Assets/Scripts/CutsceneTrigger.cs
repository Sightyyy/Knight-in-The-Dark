using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Animator targetAnimator;
    public GameObject player;
    private Collider2D col;
    [SerializeField]private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            targetAnimator.SetTrigger("PlayerWin");
            player.SetActive(false);
            col = GetComponent<Collider2D>();
            col.isTrigger = false;
        }
    }
}
