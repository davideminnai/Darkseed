using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    public float fireRate = 0.2f; // Seconds between shots
    
    [Header("Aim")]
    public LayerMask aimLayer;

    private float nextFireTime = 0f;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        
        if (firePoint == null)
        {
            Debug.LogWarning("Fire Point not assigned! Assign it in the Inspector.");
        }
    }

    public void HandleShooting()
    {
        Vector2 aimDirection = GetAimDirection();
        
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot(aimDirection);
            nextFireTime = Time.time + fireRate;
        }

        // Auto-fire when holding mouse button
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot(aimDirection);
            nextFireTime = Time.time + fireRate;
        }

        UpdateAimVisual(aimDirection);
    }

    private Vector2 GetAimDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
        
        // Clamp to prevent shooting too far down/up if needed
        return direction;
    }

    private void Shoot(Vector2 direction)
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(direction * bulletSpeed);
        }
        
        // TODO: Play shoot sound
    }

    private void UpdateAimVisual(Vector2 direction)
    {
        if (spriteRenderer == null) return;

        // Flip sprite based on aim direction
        if (direction.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.1f);
        }
    }
}