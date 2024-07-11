using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{

    public float timeSpawn;
    public WaveData[] waves;

    private GameObject currentEnemy;
    private int waveOrder = 0;
    private int currentWave = 0;
    private int currentSpawn = 0;
    private int currentSpawnEnemy = 0;
    private float timer = 0;
    private float timerSpawn = 0;
    private UIManager uiManager;

    private void Start()
    {

        uiManager = FindFirstObjectByType<UIManager>();

    }

    private void Update()
    {

        
        SpawnWave();

    }

    public void NextSpawn()
    {

        if (waveOrder < waves.Length)
        {

            waveOrder++;
            uiManager.AtualizarUI();

        }

    }

    void SpawnWave()
    {
        if (currentWave <= waveOrder)
        {

            if (currentSpawn + 1 <= waves[currentWave].spawns.Length)
            {

                if (timer > waves[currentWave].spawns[currentSpawn].timeWaiting)
                {

                    if (currentSpawnEnemy + 1 <= waves[currentWave].spawns[currentSpawn].qtd)
                    {
                        
                        if(timerSpawn > timeSpawn)
                        {
                            currentSpawnEnemy++;
                            timerSpawn = 0;
                            currentEnemy = Instantiate(waves[currentWave].spawns[currentSpawn].enemy, waves[currentWave].spawns[currentSpawn].rota[0].position, waves[currentWave].spawns[currentSpawn].rota[0].rotation);
                            currentEnemy.GetComponent<Enemy>().SetRota(waves[currentWave].spawns[currentSpawn].rota);

                        }
                        else
                        {

                            timerSpawn += 1 * Time.deltaTime;

                        }

                    }
                    else
                    {

                        timer = 0;
                        currentSpawnEnemy = 0;
                        currentSpawn++;

                    }

                }
                else
                {

                    timer += 1 * Time.deltaTime;

                }
            }
            else
            {

                currentSpawn = 0;
                currentWave++;

            }

        }

    }
    
    public int GetCurrentWave()
    {

        return waveOrder;

    }


    [Serializable]
    public class WaveData
    {
        public SpawnData[] spawns;
    }


    [Serializable]
    public class SpawnData
    {

        [Header("Tempo antes do spawn.")]
        public float timeWaiting;

        public GameObject enemy;
        public int qtd;
        public Transform[] rota;

    }

}