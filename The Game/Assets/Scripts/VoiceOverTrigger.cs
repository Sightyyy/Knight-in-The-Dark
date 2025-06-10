using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    private AudioCollection audioCollection;

    [Tooltip("Nama AudioClip VoiceOver di AudioCollection (misal: vo1, vo2, dll)")]
    public string voName;

    private bool hasTriggered = false;

    void Awake()
    {
        audioCollection = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioCollection>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;

        AudioClip clip = audioCollection.GetClipByName(voName);
        if (clip != null)
        {
            audioCollection.PlayVO(clip);
        }
        else
        {
            Debug.LogWarning($"VoiceOver '{voName}' tidak ditemukan di AudioCollection untuk {gameObject.name}");
        }
    }
}
