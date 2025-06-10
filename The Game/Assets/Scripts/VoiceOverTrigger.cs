using System.Collections;
using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    private AudioCollection audioCollection;

    PlayerCollider playerCollider;

    [Tooltip("Nama AudioClip VoiceOver di AudioCollection (misal: vo1, vo2, dll)")]
    public AudioClip clip;

    private bool hasTriggered = false;

    void Awake()
    {
        audioCollection = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioCollection>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollider>();

    }

    private void Update()
    {
        if (playerCollider.isDying)
        {
            audioCollection.StopVO();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;
        hasTriggered = true;
        if (clip != null)
        {
            audioCollection.StopPlayBGM();
            audioCollection.StopPlayVO();
            audioCollection.StopSFX();
            StartCoroutine(Wait(1));
        }
        else
        {
            Debug.LogWarning($"VoiceOver '{clip}' tidak ditemukan di AudioCollection untuk {gameObject.name}");
        }
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        audioCollection.PlayVO(clip);
    }
}
