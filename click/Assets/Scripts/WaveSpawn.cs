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
    private int spawnOrder = 0;


    public void NextSpawn()
    {
        SpawnWave(spawnOrder);
        spawnOrder++;
    }

    void SpawnWave(int waveOrder)
    {
        if(waveOrder > waves.Length)
        {

            return;

        }
        else
        {
            Debug.Log("Else");
            for (int i = 0; i < waves[waveOrder].spawns.Length; i++)
            {
                /*
                for (float time = 0; time < waves[waveOrder].spawns[i].timeWaiting;)
                {

                    time =+ 1 * Time.deltaTime;
                    Debug.Log("Timer 1" + time);

                }*/

                for (int qtd = 0; qtd < waves[waveOrder].spawns[i].timeWaiting; qtd++)
                {

                    currentEnemy = Instantiate(waves[waveOrder].spawns[i].enemy, transform.position, transform.rotation);
                    currentEnemy.GetComponent<Enemy>().SetRota(waves[waveOrder].spawns[i].rota);

                    
                    /*
                    for (float time = 0; time < timeSpawn;)
                    {

                        time = +1 * Time.deltaTime;

                    }
                    */
                }

            }
        }

    }

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