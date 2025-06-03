using UnityEngine;
using TMPro;

public class CheckpointNotification : MonoBehaviour
{
    [Header("Text Settings")]
    public GameObject textObject;
    public float moveUpDistance = 1f;
    public float duration = 2f;

    private TMP_Text tmpText;
    private CanvasGroup canvasGroup;
    public bool hasTriggered = false;

    private Vector3 initialLocalPos;

    private void Start()
    {
        if (textObject != null)
        {
            tmpText = textObject.GetComponent<TMP_Text>();
            canvasGroup = textObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = textObject.AddComponent<CanvasGroup>();

            initialLocalPos = textObject.transform.localPosition;

            textObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            ShowNotification();
        }
    }

    private void ShowNotification()
    {
        StopAllCoroutines();

        textObject.SetActive(true);
        textObject.transform.localPosition = initialLocalPos;
        canvasGroup.alpha = 1f;

        StartCoroutine(AnimateText());
    }

    private System.Collections.IEnumerator AnimateText()
    {
        Vector3 start = initialLocalPos;
        Vector3 end = start + Vector3.up * moveUpDistance;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            textObject.transform.localPosition = Vector3.Lerp(start, end, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        textObject.SetActive(false);
    }
}
