using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector2 offset = new Vector2(0f, 0f);
    [Range(0.01f, 1f)] public float smoothTime = 0.15f;

    [Header("Boundaries")]
    public bool useBounds = true;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is missing!");
            return;
        }

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            -10f
        );

        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.green;
            Vector3 center = new Vector3(
                (minBounds.x + maxBounds.x) / 2,
                (minBounds.y + maxBounds.y) / 2,
                0f);
            Vector3 size = new Vector3(
                maxBounds.x - minBounds.x,
                maxBounds.y - minBounds.y,
                1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
