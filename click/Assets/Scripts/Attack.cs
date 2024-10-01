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

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= atkSpeed)
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

            timer = 0f;  // Reset timer after attack
        }
    }

    private void AttackAllEnemies()
    {
        // Remove any enemies that have been destroyed before attacking
        enemiesInRange.RemoveAll(enemy => enemy == null);

        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                enemy.TookDamage(atkDamage);
            }
        }

        // Trigger attack animations and play sound
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
        // Remove destroyed enemies before setting a new target
        enemiesInRange.RemoveAll(enemy => enemy == null);

        if (enemiesInRange.Count > 0)
        {
            // Assuming enemies move in a line, the next target is the closest in the list
            enemiesInRange.Sort((a, b) =>
            {
                if (a == null || b == null) return 0;  // Ensure no null comparison
                return a.transform.position.z.CompareTo(b.transform.position.z);
            });

            currentTarget = enemiesInRange[0];
        }
        else
        {
            currentTarget = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
            if (currentTarget == enemy)
            {
                currentTarget = null;  // Clear target if it left the area
            }
        }
    }

    public void SetAtk(float atkSpeed, int atkDamage, Animator t1, Animator t2, Animator t3, AttackType attackType)
    {
        this.atkSpeed = atkSpeed;
        this.atkDamage = atkDamage;
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
        this.attackType = attackType;
    }
}

