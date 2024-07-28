using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour {

    public Transform[] rota;
	public int rotaCount = 0;
	public int maxHealth;
    public int currentHealth;
	public int danoZigurate;


    [Header("Drop de Item")]
	public string itemDrop;
	public int qtdDrop;

    private NavMeshAgent navMesh;

	private Rigidbody rb;
	protected Animator anim;
	protected bool isDead = false;
	private GameManager	gameManager;
	private UIManager uiManager;


    void Start () 
	{

        navMesh = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
        navMesh.SetDestination(rota[rotaCount].position);
        uiManager = FindFirstObjectByType<UIManager>();

    }
	

	void Update () 
	{

        //anim.SetFloat("Velocidade", navMesh.velocity.magnitude);

    }

	void FixedUpdate()
	{
	
        if(!isDead) AiRota();

    }

	void AiRota()
	{     

        if (navMesh.remainingDistance < navMesh.stoppingDistance+1)
            {
								
			if(rotaCount >= rota.Length)
			{
				gameManager.SetHpZigurate(danoZigurate);
                uiManager.AtualizarUI();
                DisableEnemy();
            }
			else
			{
                navMesh.SetDestination(rota[rotaCount].position);
                rotaCount++;
            }

            }
        
    }

    public void TookDamage(int damage)
	{
		if (!isDead)
		{

			currentHealth -= damage;
			anim.SetTrigger("HitDamage");
			if(currentHealth <= 0)
			{

				isDead = true;
				navMesh.enabled = false;
				gameManager.SetInventario(itemDrop, qtdDrop);
				uiManager.AtualizarUI();
				anim.SetBool("Death", true);

            }
		}
	}
	
	public void DisableEnemy()
	{

		Destroy(gameObject);

	}

	public int GetHealth()
	{

		return currentHealth;

	}

	public bool GetIsDead()
	{
		return isDead;

    }

	public void SetRota(Transform[] enemyRota)
	{

		rota = enemyRota;

	}

}
