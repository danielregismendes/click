using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float atkSpeed;
    private int atkDamage;
    private float timer;
    private bool canHit = true;
    private Animator t1, t2, t3;



    private void OnTriggerStay(Collider other)
    {
        
        Enemy[] enemy = other.GetComponents<Enemy>();

        if (!canHit)
        {
            timer += 1 * Time.deltaTime;
        }

        if (!canHit & timer > atkSpeed)
        {
            timer = 0;
            canHit = true;
        }

        if (enemy[0] != null & canHit)
        {
            for (int i = 0; i < enemy.Length; i++)
            {

                enemy[i].TookDamage(atkDamage);
                t1.SetTrigger("Attack");
                t2.SetTrigger("Attack");
                t3.SetTrigger("Attack");
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_dano, gameObject.transform.position);

            }
            
            canHit = false;

        }

    }

    public void SetAtk(float atkSpeed, int atkDamage, Animator t1, Animator t2, Animator t3)
    {
        this.atkSpeed = atkSpeed; this.atkDamage = atkDamage;
        this.t1 = t1; this.t2 = t2; this.t3 = t3;
    }

}
