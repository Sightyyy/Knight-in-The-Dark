using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Animator targetAnimator;
    public GameObject player;
    public GameObject UI;
    private Collider2D col;
    GameMenu gameMenu;
    [SerializeField] private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            targetAnimator.SetTrigger("PlayerWin");
            player.SetActive(false);
            UI.SetActive(false);
            col = GetComponent<Collider2D>();
            col.isTrigger = false;
            StartCoroutine(Return());
        }
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(25);
        gameMenu = GameObject.FindGameObjectWithTag("Admin").GetComponent<GameMenu>();
        gameMenu.ReturnToMainMenu("MainMenu");
    }
}
