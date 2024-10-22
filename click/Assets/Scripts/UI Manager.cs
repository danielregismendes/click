using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("UI Recursos")]
    public Text recVermelho;
    public Text recAzul;
    public Text recAmarelo;

    [Header("UI Gameplay")]
    public TextMeshProUGUI hpZigurate;
    public Text wave;

    private GameManager gameManager;
    private WaveSpawn waveSpawn;

    private void Start()
    {

        gameManager = FindFirstObjectByType<GameManager>();
        waveSpawn = FindFirstObjectByType<WaveSpawn>();

        AtualizarUI();

    }

    public void AtualizarUI()
    {

        recVermelho.text = gameManager.GetInventario("Corpo Vermelho").ToString();
        recAzul.text = gameManager.GetInventario("Corpo Azul").ToString();
        recAmarelo.text = gameManager.GetInventario("Corpo Amarelo").ToString();

        hpZigurate.text = gameManager.GetHpZigurate().ToString();
        wave.text = waveSpawn.GetCurrentWave() + "/" + (waveSpawn.waves.Length -1).ToString();

    }


    public void GameOver()
    {

        gameManager.GameOver();

    }

    public void Win()
    {

        gameManager.Win();

    }

    public void ZerarGame()
    {

        gameManager.ZerarGame();

    }

    public void NexSpawn()
    {

        waveSpawn.NextSpawn();

    }

}
