using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtMenuInicial : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {

        AudioManager.instance.InitializeMusic(FMODEvents.instance.music_eventos);

        gameManager = FindFirstObjectByType<GameManager>();

    }

    public void NewGame()
    {

        SceneManager.LoadScene(2);

    }

    public void Creditos()
    {

        SceneManager.LoadScene(1);

    }

    public void Exit()
    {

        Application.Quit();

    }

}
