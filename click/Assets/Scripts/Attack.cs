using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float atkSpeed;
    private int atkDamage;
    private float timer;
    private bool canHit = true;



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
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_dano, gameObject.transform.position);

            }
            
            canHit = false;

        }

    }

    public void SetAtk(float atkSpeed, int atkDamage)
    {
        this.atkSpeed = atkSpeed; this.atkDamage = atkDamage;
    }

}
