using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawn : MonoBehaviour
{

    public WaveData[] waves;

    private Vector3 spawnPosition;


    private void Start()
    {
        
        spawnPosition = transform.position;

    }

    void SpawnWave(int waveOrder)
    {

        for(int i = 0; i < waves[waveOrder].spawns.Length; i++)
        {
            
            for (float time = 0; time < waves[waveOrder].spawns[i].timeWaiting; time = +1 * Time.deltaTime)
            {

                

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