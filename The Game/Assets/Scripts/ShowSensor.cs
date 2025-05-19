using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowSensored : MonoBehaviour
{
    private Tilemap tilemap;
    
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("ShowSensored script requires a Tilemap component!");
            Destroy(this);
        }
    }

    public void StartFade(float startAlpha, float duration)
    {
        StartCoroutine(FadeEffect(startAlpha, duration));
    }

    IEnumerator FadeEffect(float startAlpha, float duration)
    {
        Color originalColor = tilemap.color;
        Color startColor = new Color(originalColor.r, originalColor.g, originalColor.b, startAlpha);
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        tilemap.color = startColor;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            tilemap.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tilemap.color = targetColor;
    }
}