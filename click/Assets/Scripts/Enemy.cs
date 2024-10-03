﻿using FMOD.Studio;
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

    public float originalSpeed;
    public bool isSlowed = false;
    private NavMeshAgent navMesh;
	protected Animator anim;
	public bool isDead = false;
	private GameManager	gameManager;
	private UIManager uiManager;
	private int bonusDrop = 0;
    private EventInstance enemyFootsteps;

    void Start () 
	{
        navMesh = GetComponent<NavMeshAgent>();
        originalSpeed = navMesh.speed;  // Store the initial speed
        anim = GetComponent<Animator>();
		currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
        navMesh.SetDestination(rota[rotaCount].position);
        uiManager = FindFirstObjectByType<UIManager>();
		SetBonusRelic();
        enemyFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.inimigo_steps);
        enemyFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        enemyFootsteps.start();

    }

    private void Update()
    {
        
        if(gameManager.stage == STAGEFASE.GAMEOVER) DisableEnemy();

    }

    void FixedUpdate()
	{
	
        AiRota();
        UpdateSound();

    }

	void AiRota()
	{
        if (!isDead)
        {

            if (navMesh.remainingDistance < navMesh.stoppingDistance + 1)
            {

                if (rotaCount >= rota.Length)
                {
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.litch_dano, gameObject.transform.position);
                    gameManager.SetHpZigurate(danoZigurate);
                    uiManager.AtualizarUI();
                    enemyFootsteps.setPaused(true);
                    DisableEnemy();
                }
                else
                {
                    navMesh.SetDestination(rota[rotaCount].position);
                    rotaCount++;
                }

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
                enemyFootsteps.stop(STOP_MODE.IMMEDIATE);
                enemyFootsteps.release();
                isDead = true;
				navMesh.enabled = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_morte, gameObject.transform.position);
                gameManager.SetInventario(itemDrop, qtdDrop + bonusDrop);
				uiManager.AtualizarUI();
				anim.SetBool("Death", true);

            }
		}
	}

    public void ApplySlow(float slowPercentage)
    {
        if (!isSlowed)
        {
            originalSpeed = navMesh.speed; // Store the original speed
            navMesh.speed = originalSpeed * (1 - slowPercentage / 100f); // Set the reduced speed directly

            // Force reactivation to ensure speed change takes effect
            navMesh.isStopped = true;
            navMesh.isStopped = false;

            Debug.Log($"Applying slow: Original Speed = {originalSpeed}, New Speed = {navMesh.speed}"); // Debug line
            isSlowed = true;
        }
    }



    public void RemoveSlow()
    {
        if (isSlowed)
        {
            navMesh.speed = originalSpeed; // Restore original speed
            Debug.Log($"Removing slow: Restored Speed = {originalSpeed}"); // Debug line
            isSlowed = false;
        }
    }




    public void DisableEnemy()
	{

        enemyFootsteps.stop(STOP_MODE.IMMEDIATE);
        enemyFootsteps.release();

        if (isDead)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_morte_contatoaosolo, Camera.main.transform.position);

        }

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

    public void SetBonusRelic()
    {

        for (int iRelic = 0; iRelic < gameManager.reliquias.Count; iRelic++)
        {

            if (gameManager.reliquias[iRelic].tower != "")
            {


            }
            else
            {

                if (itemDrop == gameManager.reliquias[iRelic].tipoRecurso)
                {

                    bonusDrop += gameManager.reliquias[iRelic].drop;

                }

            }

        }

    }

    private void UpdateSound()
    {
    
        enemyFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

    }

}
