using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtWave : MonoBehaviour
{
    public WaveSpawn waveSpawn;


    public void NextWave()
    {
        waveSpawn.NextSpawn();
    }

}
