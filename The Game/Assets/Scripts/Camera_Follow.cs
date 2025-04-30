using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Follow")]
    public Transform target;
    [Range(0.1f, 10f)] public float smoothSpeed = 5f;
    public Vector2 offset = new Vector2(0, 0);

    [Header("Camera Boundaries")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Pastikan posisi Z kamera 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is missing!");
            return;
        }

        // Target position dengan offset (Z tetap 0)
        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            0); // Z selalu 0

        // Gunakan SmoothDamp untuk pergerakan halus
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothSpeed * Time.deltaTime);

        // Apply boundaries jika aktif
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        // Pastikan Z tetap 0
        smoothedPosition.z = 0;
        transform.position = smoothedPosition;
    }

    // Visualisasi boundaries di editor
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3(
                (minBounds.x + maxBounds.x) / 2,
                (minBounds.y + maxBounds.y) / 2,
                0);
            Vector3 size = new Vector3(
                maxBounds.x - minBounds.x,
                maxBounds.y - minBounds.y,
                1);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
