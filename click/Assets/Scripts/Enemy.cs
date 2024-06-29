﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour {

    public Transform[] rota;
	public int rotaCount = 0;

    public float velocidade;
    public float aceleracao;
    public float velInvestida;
    public float aceInvestida;
    public float timerAtaque;
    public float maxSpeed;
	public int maxHealth;
	public AudioClip collisionSound, deathSound;
    public float maxSlope;
    public LayerMask layerGround;
    public Transform groundCheck;
    public int currentHealth;

	private NavMeshAgent navMesh;

    private float currentSpeed;
	private Rigidbody rb;
	protected Animator anim;
	protected bool facingRight = false;
	private Transform target;
	protected bool isDead = false;
	private float zForce;
	private float walkTimer;
	private bool damaged = false;
	private AudioSource audioS;
	//private GameManager	gameManager;


    void Start () {

        navMesh = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		currentHealth = maxHealth;
        //gameManager = FindFirstObjectByType<GameManager>();
        velocidade = navMesh.speed;
        aceleracao = navMesh.acceleration;

        navMesh.SetDestination(rota[rotaCount].position);

    }
	

	void Update () 
	{

        //anim.SetFloat("Velocidade", navMesh.velocity.magnitude);

    }

	void FixedUpdate()
	{

        AiRota();

    }

	void AiRota()
	{     

        if (navMesh.remainingDistance < navMesh.stoppingDistance+1)
            {
				navMesh.SetDestination(rota[rotaCount].position);
				rotaCount++;
            }
        
    }

   
    void LookTargert(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMesh.angularSpeed);
    }


    public void TookDamage(int damage)
	{
		if (!isDead)
		{
			damaged = true;
			currentHealth -= damage;
			//anim.SetTrigger("HitDamage");
			if(currentHealth <= 0)
			{
				isDead = true;
				rb.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);
				DisableEnemy();

            }
		}
	}
	
	public void DisableEnemy()
	{
		Destroy(gameObject);
	}

	void ResetSpeed()
	{
		currentSpeed = maxSpeed;
    }
    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    [Serializable]
    public class EnemyHit
	{
		public int damage = 1;
		public AudioClip collisionSound;
        public float knockback;
    }

	public int GetHealth()
	{
		return currentHealth;
	}

	public bool GetIsDead()
	{
		return isDead;

    }

}
