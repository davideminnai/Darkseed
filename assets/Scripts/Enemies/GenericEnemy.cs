using UnityEngine;

public class GenericEnemy : EnemyController
{
    [Header("Patrol Settings")]
    public float patrolSpeed = 2f;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Chase Settings")]
    public float chaseSpeed = 4f;
    
    [Header("Attack Settings")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float attackRange = 3f;
    public float fireRate = 1.5f;
    private float nextFireTime = 0f;

    private enum EnemyState { Patrol, Chase, Attack }
    private EnemyState currentState = EnemyState.Patrol;

    protected override void Update()
    {
        base.Update();
        
        if (!enemyHealth.isAlive || playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                if (distanceToPlayer <= detectionRange)
                    ChangeState(EnemyState.Chase);
                break;
                
            case EnemyState.Chase:
                ChaseBehavior(distanceToPlayer);
                if (distanceToPlayer <= attackRange)
                    ChangeState(EnemyState.Attack);
                else if (distanceToPlayer > detectionRange * 1.5f)
                    ChangeState(EnemyState.Patrol);
                break;
                
            case EnemyState.Attack:
                AttackBehavior(distanceToPlayer);
                if (distanceToPlayer > attackRange * 1.2f)
                    ChangeState(EnemyState.Chase);
                else if (distanceToPlayer > detectionRange * 1.5f)
                    ChangeState(EnemyState.Patrol);
                break;
        }

        UpdateAnimation();
    }

    private void PatrolBehavior()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        
        rb.velocity = new Vector2(direction.x * patrolSpeed, rb.velocity.y);
        
        // Flip sprite to face movement direction
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // Check if reached patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void ChaseBehavior(float distanceToPlayer)
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
        
        // Keep facing player
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void AttackBehavior(float distanceToPlayer)
    {
        // Stop moving and aim at player
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        if (Time.time >= nextFireTime && firePoint != null && enemyBulletPrefab != null)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootAtPlayer()
    {
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(direction * 8f, false); // false = enemy bullet
        }
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
        
        switch (currentState)
        {
            case EnemyState.Patrol:
                animator?.SetInteger("state", 0);
                break;
            case EnemyState.Chase:
                animator?.SetInteger("state", 1);
                break;
            case EnemyState.Attack:
                animator?.SetInteger("state", 2);
                break;
        }
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        
        // Knockback effect
        Vector2 knockback = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized * 3f;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        
        // Brief stun
        StartCoroutine(StunCoroutine(0.3f));
    }

    private System.Collections.IEnumerator StunCoroutine(float duration)
    {
        float originalSpeed = chaseSpeed;
        chaseSpeed = 0;
        yield return new WaitForSeconds(duration);
        chaseSpeed = originalSpeed;
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        
        if (animator != null)
        {
            animator.SetBool("isShooting", currentState == EnemyState.Attack);
            animator.SetFloat("speed", rb.velocity.x != 0 ? Mathf.Abs(rb.velocity.x) : 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, 0.3f);
            }
        }
    }
}