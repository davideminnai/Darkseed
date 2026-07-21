using UnityEngine;

public class BossEnemy : EnemyController
{
    [Header("Boss Phases")]
    public int maxPhase = 3;
    private int currentPhase = 1;
    
    [Header("Phase Transitions")]
    public float phaseTransitionHealthPercent = 0.33f; // Health % to trigger next phase
    
    [Header("Phase 1 - Patrol & Shoot")]
    public float phase1PatrolSpeed = 2f;
    public float phase1FireRate = 1f;
    
    [Header("Phase 2 - Aggressive Chase")]
    public float phase2ChaseSpeed = 5f;
    public float phase2FireRate = 0.5f;
    
    [Header("Phase 3 - Enraged")]
    public float phase3Speed = 6f;
    public float phase3FireRate = 0.3f;
    public GameObject explosionPrefab;
    
    [Header("Special Attacks")]
    public Transform[] spawnPoints;
    public GameObject enemyBulletPrefab;
    public float specialAttackCooldown = 10f;
    private float nextSpecialAttackTime = 0f;

    private enum BossState { Idle, Patrol, Chase, Attack, SpecialAttack }
    private BossState currentState = BossState.Idle;

    protected override void Update()
    {
        base.Update();
        
        if (!enemyHealth.isAlive || playerTransform == null) return;

        CheckPhaseTransition();
        ExecuteCurrentState();
        UpdateAnimation();
    }

    private void CheckPhaseTransition()
    {
        float healthPercent = (float)enemyHealth.currentHealth / enemyHealth.maxHealth;
        
        if (healthPercent <= phaseTransitionHealthPercent && currentPhase < maxPhase)
        {
            EnterNextPhase();
        }
    }

    private void EnterNextPhase()
    {
        currentPhase++;
        
        // TODO: Play phase transition effect/sound
        Debug.Log($"Boss entered Phase {currentPhase}!");
        
        switch (currentState)
        {
            case BossState.Idle:
                ChangeState(BossState.Patrol);
                break;
            case BossState.Patrol:
                ChangeState(BossState.Chase);
                break;
            default:
                ChangeState(BossState.Attack);
                break;
        }
    }

    private void ExecuteCurrentState()
    {
        switch (currentState)
        {
            case BossState.Idle:
                IdleBehavior();
                break;
            case BossState.Patrol:
                PatrolBehavior();
                break;
            case BossState.Chase:
                ChaseBehavior();
                break;
            case BossState.Attack:
                AttackBehavior();
                break;
            case BossState.SpecialAttack:
                SpecialAttackBehavior();
                break;
        }

        // Check for special attack opportunity
        if (Time.time >= nextSpecialAttackTime && currentState != BossState.SpecialAttack)
        {
            if (Random.value < 0.3f) // 30% chance each frame
            {
                ChangeState(BossState.SpecialAttack);
                nextSpecialAttackTime = Time.time + specialAttackCooldown;
            }
        }
    }

    private void IdleBehavior()
    {
        rb.velocity = Vector2.zero;
        
        // Wait a moment then transition to patrol
        if (Random.value < 0.01f)
        {
            ChangeState(BossState.Patrol);
        }
    }

    private void PatrolBehavior()
    {
        float speed = GetSpeedForPhase(phase1PatrolSpeed);
        
        // Move towards player but keep some distance
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer > 3f)
        {
            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-direction.x * speed * 0.5f, rb.velocity.y); // Back away slightly
        }

        // Face player
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // Transition to attack when close enough
        if (distanceToPlayer <= 2f)
        {
            ChangeState(BossState.Attack);
        }
    }

    private void ChaseBehavior()
    {
        float speed = GetSpeedForPhase(phase2ChaseSpeed);
        
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Face player
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // Transition to attack when very close
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= 1.5f)
        {
            ChangeState(BossState.Attack);
        }
    }

    private void AttackBehavior()
    {
        float fireRate = GetFireRateForPhase(phase1FireRate);
        
        // Stop and shoot at player
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        if (Time.time >= nextFireTime && firePoint != null && enemyBulletPrefab != null)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + fireRate;
        }

        // Transition back to chase after attacking
        if (Random.value < 0.02f)
        {
            ChangeState(BossState.Chase);
        }
    }

    private void SpecialAttackBehavior()
    {
        // Charge at player then explode
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(direction.x * phase3Speed, rb.velocity.y);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= 1f)
        {
            PerformExplosion();
            ChangeState(BossState.Idle);
        }
    }

    private void ShootAtPlayer()
    {
        Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        
        // Spread shot in phase 3
        if (currentPhase >= 3)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector2 spreadDirection = direction;
                spreadDirection.y += i * 0.3f;
                spreadDirection.Normalize();
                
                GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.Initialize(spreadDirection * 10f, false);
                }
            }
        }
        else
        {
            // Single shot in earlier phases
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(direction * 10f, false);
            }
        }
    }

    private void PerformExplosion()
    {
        // TODO: Spawn explosion effect
        Debug.Log("Boss performed special attack!");
        
        // Damage player if nearby
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= 3f)
        {
            PlayerController player = playerTransform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    private float GetSpeedForPhase(float baseSpeed)
    {
        switch (currentPhase)
        {
            case 2: return phase2ChaseSpeed;
            case 3: return phase3Speed;
            default: return baseSpeed;
        }
    }

    private float GetFireRateForPhase(float baseFireRate)
    {
        switch (currentPhase)
        {
            case 2: return phase2FireRate;
            case 3: return phase3FireRate;
            default: return baseFireRate;
        }
    }

    private void ChangeState(BossState newState)
    {
        currentState = newState;
        
        switch (currentState)
        {
            case BossState.Idle:
                animator?.SetInteger("state", 0);
                break;
            case BossState.Patrol:
                animator?.SetInteger("state", 1);
                break;
            case BossState.Chase:
                animator?.SetInteger("state", 2);
                break;
            case BossState.Attack:
                animator?.SetInteger("state", 3);
                break;
            case BossState.SpecialAttack:
                animator?.SetInteger("state", 4);
                break;
        }
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        
        // Knockback effect (reduced for boss)
        Vector2 knockback = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized * 1.5f;
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        
        if (animator != null)
        {
            animator.SetBool("isShooting", currentState == BossState.Attack || currentState == BossState.SpecialAttack);
            animator.SetFloat("speed", rb.velocity.x != 0 ? Mathf.Abs(rb.velocity.x) : 0);
            animator.SetInteger("phase", currentPhase);
        }
    }

    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        
        // Draw spawn points for special attack
        if (spawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, 0.5f);
            }
        }
    }
}