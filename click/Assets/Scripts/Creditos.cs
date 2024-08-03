using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creditos : MonoBehaviour
{

    void Start()
    {

        AudioManager.instance.InitializeMusic(FMODEvents.instance.music_eventos);

    }

    public void LeaveCreditos()
    {

        SceneManager.LoadScene(0);

    }

}
