using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Area,
    SingleTarget
}

public class Attack : MonoBehaviour
{
    private float atkSpeed;
    private int atkDamage;
    private float timer;
    private List<Enemy> enemiesInRange = new List<Enemy>();  // Track all enemies in range
    private Enemy currentTarget;
    private Animator t1, t2, t3;
    private AttackType attackType;
    
    public float slowPercentage = 0.3f; // Default value, can be overridden
    public float updateInterval = 0.5f; // Time interval for applying slow effect

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(atkSpeed);

            if (enemiesInRange.Count > 0) // Only attack if there are enemies
            {
                switch (attackType)
                {
                    case AttackType.Area:
                        AttackAllEnemies();
                        break;
                    case AttackType.SingleTarget:
                        AttackSingleEnemy();
                        break;
                }
            }
        }
    }

    private IEnumerator ApplySlowdownCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // Slowdown update interval

            foreach (Enemy enemy in enemiesInRange)
            {
                if (enemy != null && !enemy.isSlowed)
                {
                    enemy.ApplySlow(slowPercentage);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
            if (!enemy.isSlowed)
            {
                enemy.ApplySlow(slowPercentage);
                Debug.Log("Applying slow to enemy: " + enemy.name); // Debug line
            }

            // Start attacking if not already attacking
            if (enemiesInRange.Count == 1)
            {
                StartCoroutine(AttackCoroutine());
                StartCoroutine(ApplySlowdownCoroutine());
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
            if (enemy.isSlowed)
            {
                enemy.RemoveSlow();
            }

            // Stop coroutines only if no enemies are left
            if (enemiesInRange.Count == 0)
            {
                StopCoroutine(AttackCoroutine());
                StopCoroutine(ApplySlowdownCoroutine());
            }
        }
    }


    private void AttackAllEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || enemy.isDead);  // Clean up list

        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.TookDamage(atkDamage);
            }
        }

        // Trigger animations once
        TriggerAttackAnimationAndSound();
    }

    private void TriggerAttackAnimationAndSound()
    {
        t1.SetTrigger("Attack");
        t2.SetTrigger("Attack");
        t3.SetTrigger("Attack");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_dano, gameObject.transform.position);
    }


    private void AttackSingleEnemy()
    {
        // Remove destroyed enemies
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (currentTarget == null || !enemiesInRange.Contains(currentTarget))
        {
            SetNewTarget();
        }

        if (currentTarget != null)
        {
            currentTarget.TookDamage(atkDamage);

            // Trigger attack animations and play sound
            t1.SetTrigger("Attack");
            t2.SetTrigger("Attack");
            t3.SetTrigger("Attack");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_dano, gameObject.transform.position);
        }
    }

    private void SetNewTarget()
    {
        currentTarget = null;
        float minDistance = float.MaxValue;

        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentTarget = enemy;
                }
            }
        }
    }
            
    private void CleanUpEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || enemy.isDead);
    }


    private void ApplySlowdown(Enemy enemy)
    {
        // Ensure the slow effect is applied only once or is reapplied only after a long interval
        if (!enemy.isSlowed)
        {
            enemy.ApplySlow(slowPercentage);  // Apply the slow effect
        }
    }


    private void RemoveSlowdown(Enemy enemy)
    {
        if (enemy.isSlowed)
        {
            enemy.RemoveSlow(); // Restore original speed
        }
    }


    public void SetAtk(float atkSpeed, int atkDamage, Animator t1, Animator t2, Animator t3, AttackType attackType, float slowPercentage)
    {
        this.atkSpeed = atkSpeed;
        this.atkDamage = atkDamage;
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.attackType = attackType;
        this.slowPercentage = slowPercentage; // Correctly assigning the incoming parameter to the field.
    }

}

