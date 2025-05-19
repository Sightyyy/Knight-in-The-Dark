using UnityEngine;

public class DelayedFollowCamera : MonoBehaviour
{
    [Header("Target Follow")]
    public Transform target;
    [Range(0.1f, 10f)] public float smoothSpeed = 5f;
    public Vector2 offset = new Vector2(0, 0);
    public float followThreshold = 0.5f; // Jarak sebelum kamera mulai mengikuti

    [Header("Camera Boundaries")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = new Vector3(0, 0, 0);
        transform.position = initialPosition;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is missing!");
            return;
        }

        // Cek jarak player dari pusat kamera
        float distanceFromCenter = Vector2.Distance(
            new Vector2(target.position.x, target.position.y),
            new Vector2(initialPosition.x, initialPosition.y));

        // Aktifkan follow jika player melewati threshold
        if (!isFollowing && distanceFromCenter > followThreshold)
        {
            isFollowing = true;
        }

        Vector3 targetPosition;
        
        if (isFollowing)
        {
            // Posisi target dengan offset (Z tetap 0)
            targetPosition = new Vector3(
                target.position.x + offset.x,
                target.position.y + offset.y,
                0);
        }
        else
        {
            // Tetap di posisi awal
            targetPosition = initialPosition;
        }

        // Gunakan SmoothDamp untuk pergerakan halus
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothSpeed * Time.deltaTime);

        // Apply boundaries jika aktif
        if (useBounds && isFollowing)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        // Pastikan Z tetap 0
        smoothedPosition.z = 0;
        transform.position = smoothedPosition;
    }

    void OnDrawGizmosSelected()
    {
        // Gambar threshold follow
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followThreshold);

        // Gambar boundaries
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