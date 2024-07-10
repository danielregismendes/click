using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float atkSpeed;
    private int atkDamage;
    private float timer;
    private bool canHit = true;

    private void Update()
    {
        
        if (!canHit)
        {
            timer += 1*Time.deltaTime;
        }

        if(!canHit & timer > atkSpeed)
        {
            timer = 0;
            canHit = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null & canHit)
        {

            enemy.TookDamage(atkDamage);
            canHit = false;

        }

    }

    public void SetAtk(float atkSpeed, int atkDamage)
    {
        this.atkSpeed = atkSpeed; this.atkDamage = atkDamage;
    }

}
