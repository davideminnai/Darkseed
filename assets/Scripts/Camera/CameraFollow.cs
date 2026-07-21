using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Player transform
    
    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public float lookAheadSpeed = 3f;
    public Vector3 offset = new Vector3(0, 1.5f, -10f);
    
    [Header("Boundaries")]
    public bool useBounds = true;
    public Vector2 minBounds = new Vector2(-50f, -50f);
    public Vector2 maxBounds = new Vector2(50f, 50f);
    
    [Header("Dead Zone")]
    public float deadZoneSize = 0.5f;
    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position
        Vector3 desiredPosition = target.position + offset;
        
        // Smooth follow
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref currentVelocity, 
            1f / followSpeed
        );

        // Look ahead when moving horizontally
        float lookAhead = target.GetComponent<Rigidbody2D>()?.velocity.x ?? 0;
        smoothedPosition.x += Mathf.Sign(lookAhead) * lookAheadSpeed * Time.deltaTime;

        // Apply bounds
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        transform.position = smoothedPosition;
        
        // Keep Z position constant for 2D games
        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            -10f
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(target.position, Vector3.one * 0.5f);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.position + offset);
        }

        if (useBounds)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                new Vector3((minBounds.x + maxBounds.x) / 2, (minBounds.y + maxBounds.y) / 2),
                new Vector3(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y)
            );
        }
    }
}