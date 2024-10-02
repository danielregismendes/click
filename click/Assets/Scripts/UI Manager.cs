using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("UI Recursos")]
    public Text recAzul;

    [Header("UI Gameplay")]
    public TextMeshProUGUI hpZigurate;
    public Text wave;

    private GameManager gameManager;
    private WaveSpawn waveSpawn;

    [Header("UI Tower Prices")]
    public Text bluePrice;
    public Text redPrice;
    public Text yellowPrice;

    private void Start()
    {
        // Initialize the reference to GameManager
        gameManager = Object.FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }

        // Initialize the reference to WaveSpawn
        waveSpawn = Object.FindFirstObjectByType<WaveSpawn>();

        if (waveSpawn == null)
        {
            Debug.LogError("WaveSpawn not found in the scene!");
            return;
        }

        // Ensure all UI elements are assigned
        if (bluePrice == null || redPrice == null || yellowPrice == null || recAzul == null)
        {
            Debug.LogError("One or more UI Text elements are not assigned in the inspector.");
            return;
        }

        // Update the prices for all towers at the start
        UpdateAllTowerPricesUI();
        AtualizarUI();

    }

    public void AtualizarUI()
    {
        // Verify each UI element before accessing it
        if (recAzul == null)
        {
            Debug.LogError("recAzul is not assigned in the Inspector!");
            return;
        }
        if (hpZigurate == null)
        {
            Debug.LogError("hpZigurate is not assigned in the Inspector!");
            return;
        }
        if (wave == null)
        {
            Debug.LogError("wave is not assigned in the Inspector!");
            return;
        }
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing. Cannot update UI.");
            return;
        }
        if (waveSpawn == null)
        {
            Debug.LogError("WaveSpawn reference is missing. Cannot update wave count.");
            return;
        }

        // If all elements are correctly assigned, proceed to update the UI
        try
        {
            recAzul.text = gameManager.GetInventario("Moeda").ToString();
            hpZigurate.text = gameManager.GetHpZigurate().ToString();
            wave.text = waveSpawn.GetCurrentWave() + "/" + (waveSpawn.waves.Length - 1).ToString();
        }
        catch (System.NullReferenceException ex)
        {
            Debug.LogError("NullReferenceException encountered in AtualizarUI: " + ex.Message);
        }
    }


    private void UpdateAllTowerPricesUI()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager reference is missing. Cannot update tower prices.");
            return;
        }

        // Update the price for the Blue Tower
        TowerData blueTower = gameManager.GetTower("Blue Tower");
        if (blueTower != null)
        {
            bluePrice.text = blueTower.qtdRecurso.ToString();
        }
        else
        {
            bluePrice.text = "N/A"; // Optional: display "N/A" if tower data is not found
        }

        // Update the price for the Red Tower
        TowerData redTower = gameManager.GetTower("Red Tower");
        if (redTower != null)
        {
            redPrice.text = redTower.qtdRecurso.ToString();
        }
        else
        {
            redPrice.text = "N/A"; // Optional: display "N/A" if tower data is not found
        }

        // Update the price for the Yellow Tower
        TowerData yellowTower = gameManager.GetTower("Yellow Tower");
        if (yellowTower != null)
        {
            yellowPrice.text = yellowTower.qtdRecurso.ToString();
        }
        else
        {
            yellowPrice.text = "N/A"; // Optional: display "N/A" if tower data is not found
        }
    }



    // Example: Call this method if the prices of towers change dynamically
    public void RefreshTowerPrices()
    {
        UpdateAllTowerPricesUI();
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
