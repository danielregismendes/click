using FMOD.Studio;
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
    private bool win = false;
    private GameManager gameManager;

    private EventInstance music;
    private EventInstance ambient;

    private void Start()
    {

        uiManager = FindFirstObjectByType<UIManager>();
        gameManager = FindFirstObjectByType<GameManager>();
        AudioManager.instance.InitializeMusic(FMODEvents.instance.music_gameplay);
        AudioManager.instance.InitializeAmbience(FMODEvents.instance.ambiencia_gamplay);

    }

    private void Update()
    {        

        if (!win && gameManager.stage != STAGEFASE.GAMEOVER) SpawnWave();

        if (currentWave == waves.Length) AudioManager.instance.SetMusicParameter("situacao", 2);

        if (!win && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.N))
        {

            ClearFase();

        }

    }

    public void NextSpawn()
    {

        if (waveOrder < waves.Length)
        {

            AudioManager.instance.PlayOneShot(FMODEvents.instance.inicio_de_orda, Camera.main.transform.position);
            waveOrder++;
            uiManager.AtualizarUI();

        }

    }

    void SpawnWave()
    {
        if (currentWave==0 || currentWave < waveOrder)
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
        else
        {

            if(waveOrder == waves.Length)
            {

                Enemy enemy;
                enemy = FindAnyObjectByType<Enemy>();

                if (enemy == null)
                {

                    ClearFase();

                }

            }


        }

    }

    public void ClearFase()
    {

        Animator animCanvas = uiManager.GetComponent<Animator>();
        animCanvas.SetTrigger("Win");
        AudioManager.instance.StopMusic();
        AudioManager.instance.StopAmbient();
        AudioManager.instance.PlayOneShot(FMODEvents.instance.vinheta_vitoriaestagio, Camera.main.transform.position);
        win = true;

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