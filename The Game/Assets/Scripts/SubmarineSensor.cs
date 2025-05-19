using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class SubmarineSensor : MonoBehaviour
{
    [Header("Sensor Settings")]
    public float sensorRadius = 10f;
    public float sensorDuration = 2f;
    public float cooldownTime = 15f;
    public Color sensorColor = Color.blue;
    public KeyCode sensorKey = KeyCode.LeftShift;
    public bool useMouseClick = true;

    [Header("Tilemap Fade Settings")]
    public float initialOpacity = 0.66f;
    public float fadeDuration = 1.5f;

    private bool isSensorActive = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        // Handle cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        // Check for sensor activation
        if ((Input.GetKeyDown(sensorKey) || (useMouseClick && Input.GetMouseButtonDown(0))) 
            && !isSensorActive && !isOnCooldown)
        {
            ActivateSensor();
        }
    }

    void ActivateSensor()
    {
        if (isSensorActive || isOnCooldown) return;

        StartCoroutine(SensorEffect());
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }

    IEnumerator SensorEffect()
    {
        isSensorActive = true;
        
        // Get all Tilemap objects with "Sensored" tag within radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, sensorRadius);
        
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Sensored"))
            {
                Tilemap tilemap = collider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    // Start the fade effect on each detected tilemap
                    StartCoroutine(FadeTilemap(tilemap));
                }
            }
        }

        yield return new WaitForSeconds(sensorDuration);
        isSensorActive = false;
    }

    IEnumerator FadeTilemap(Tilemap tilemap)
    {
        Color originalColor = tilemap.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        Color startColor = new Color(originalColor.r, originalColor.g, originalColor.b, initialOpacity);

        // Set initial opacity
        tilemap.color = startColor;

        // Fade to transparent
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            tilemap.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final color is set
        tilemap.color = targetColor;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sensorRadius);
    }
}