using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataLogger : MonoBehaviour
{
    private string filePath;
    private GameManager gameManager;
    private WaveSpawn waveSpawn;
    private int startingMoeda;
    private int totalWaves;
    private int finalMoeda;
    private int gameOverWave = -1;
    private int finalZigurateHp;

    private int startingZigurateHp;  // Track Zigurate HP at the start
    private Dictionary<int, int> zigurateHpPerWave = new Dictionary<int, int>();  // Track Zigurate HP per wave

    private Dictionary<int, int> moedaSpentPerWave = new Dictionary<int, int>();
    private Dictionary<int, List<string>> towersBuiltPerWave = new Dictionary<int, List<string>>();
    private Dictionary<int, int> towersDormantToVazioPerWave = new Dictionary<int, int>();

    private void Start()
    {
        // Use Application.dataPath to save in the Assets folder
        filePath = System.IO.Path.Combine(Application.dataPath, "Dados para Analise.txt");
        Debug.Log("Saving data to: " + filePath);

        gameManager = FindFirstObjectByType<GameManager>();
        waveSpawn = FindFirstObjectByType<WaveSpawn>();

        // Collect initial stage data
        startingMoeda = gameManager.GetMoeda();
        totalWaves = waveSpawn.waves.Length;
        startingZigurateHp = gameManager.GetHpZigurate();  // Get initial Zigurate health

        Debug.Log("GameDataLogger Start: Moeda = " + startingMoeda + ", Zigurate HP = " + startingZigurateHp + ", Total Waves = " + totalWaves);

        // Test file writing at startup
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open file in append mode
            {
                writer.WriteLine("Test log: GameDataLogger started successfully.");
            }
            Debug.Log("Test log written to file.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing test log to file: " + ex.Message);
        }
    }

    public void OnWaveStart()
    {
        int currentWave = waveSpawn.GetCurrentWave();
        moedaSpentPerWave[currentWave] = 0;
        towersBuiltPerWave[currentWave] = new List<string>();
        towersDormantToVazioPerWave[currentWave] = 0;
        zigurateHpPerWave[currentWave] = gameManager.GetHpZigurate();  // Track Zigurate health per wave

        Debug.Log("Wave " + currentWave + " started. Zigurate HP: " + zigurateHpPerWave[currentWave]);

        // Write the wave start log to the file
        WriteWaveDataToFile(currentWave);
    }

    private void WriteWaveDataToFile(int waveNumber)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open file in append mode
            {
                writer.WriteLine("Wave " + waveNumber + " started.");
                writer.WriteLine("Zigurate HP at start of wave: " + zigurateHpPerWave[waveNumber]);
                writer.WriteLine("Moeda Spent: " + moedaSpentPerWave[waveNumber]);
                writer.WriteLine("--------------------------------------------------");
            }

            Debug.Log("Wave data written to file for wave: " + waveNumber);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing wave data to file: " + ex.Message);
        }
    }

    public void OnTowerBuilt(string towerName)
    {
        int currentWave = waveSpawn.GetCurrentWave();

        // Update towers built data
        if (towersBuiltPerWave.ContainsKey(currentWave))
        {
            towersBuiltPerWave[currentWave].Add(towerName);
        }
        else
        {
            towersBuiltPerWave[currentWave] = new List<string> { towerName };
        }

        Debug.Log("Tower built during wave " + currentWave + ": " + towerName);

        // Log the tower built and moeda spent data to the file
        WriteTowerBuiltDataToFile(currentWave, towerName);
    }

    private void WriteTowerBuiltDataToFile(int waveNumber, string towerName)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open file in append mode
            {
                writer.WriteLine("Tower built during wave " + waveNumber + ": " + towerName);

                // Log the moeda spent for this wave as part of tower building
                if (moedaSpentPerWave.ContainsKey(waveNumber))
                {
                    writer.WriteLine("Moeda Spent during tower build: " + moedaSpentPerWave[waveNumber]);
                }

                // Log remaining moedas after the tower was built
                writer.WriteLine("Remaining Moeda after tower build: " + gameManager.GetMoeda());
                
            }

            Debug.Log("Tower built and moeda data written to file for wave: " + waveNumber);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing tower built data to file: " + ex.Message);
        }
    }


    public void OnTowerTransitionDormantToVazio()
    {
        int currentWave = waveSpawn.GetCurrentWave();

        // Update tower state transition data
        if (towersDormantToVazioPerWave.ContainsKey(currentWave))
        {
            towersDormantToVazioPerWave[currentWave]++;
        }
        else
        {
            towersDormantToVazioPerWave[currentWave] = 1;
        }

        Debug.Log("Tower transitioned from DORMENTE to VAZIO during wave " + currentWave);

        // Log the tower transition and moeda spent data to the file
        WriteTowerTransitionDataToFile(currentWave);
    }

    private void WriteTowerTransitionDataToFile(int waveNumber)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open file in append mode
            {
                writer.WriteLine("Tower transitioned from DORMENTE to VAZIO during wave " + waveNumber);

                // Log the moeda spent for this wave as part of the transition
                if (moedaSpentPerWave.ContainsKey(waveNumber))
                {
                    writer.WriteLine("Moeda Spent during tower transition: " + moedaSpentPerWave[waveNumber]);
                }

                // Log remaining moedas after the tower state transitioned
                writer.WriteLine("Remaining Moeda after tower transition: " + gameManager.GetMoeda());

            }

            Debug.Log("Tower transition and moeda data written to file for wave: " + waveNumber);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing tower transition data to file: " + ex.Message);
        }
    }




    public void OnGameOver()
    {
        // Get the current wave when game over occurs
        gameOverWave = waveSpawn.GetCurrentWave();
        Debug.Log("Game over occurred on wave: " + gameOverWave);

        // Log stage end data
        OnStageEnd();
    }

    public void OnStageEnd()
    {
        finalMoeda = gameManager.GetMoeda();
        finalZigurateHp = gameManager.GetHpZigurate();  // Get Zigurate health at the end
        Debug.Log("Stage ended. Final Moeda = " + finalMoeda + ", Final Zigurate HP = " + finalZigurateHp);

        WriteDataToFile();
    }

    private void WriteDataToFile()
    {
        try
        {
            Debug.Log("Writing data to file: " + filePath);

            using (StreamWriter writer = new StreamWriter(filePath, true)) // Open file in append mode
            {
                writer.WriteLine("Stage Start:");
                writer.WriteLine("Starting Moeda: " + startingMoeda);
                writer.WriteLine("Starting Zigurate HP: " + startingZigurateHp);
                writer.WriteLine("Total Waves: " + totalWaves);

                // Loop through waves and log data
                foreach (var waveData in moedaSpentPerWave)
                {
                    int waveNumber = waveData.Key;
                    writer.WriteLine("Wave " + waveNumber + ":");
                    writer.WriteLine("Moeda Spent: " + waveData.Value);

                    // Log towers built during the wave
                    if (towersBuiltPerWave.ContainsKey(waveNumber))
                    {
                        writer.WriteLine("Towers Built: " + string.Join(", ", towersBuiltPerWave[waveNumber]));
                    }

                    // Log towers transitioning from DORMENTE to VAZIO
                    if (towersDormantToVazioPerWave.ContainsKey(waveNumber))
                    {
                        writer.WriteLine("Towers Transitioned from DORMENTE to VAZIO: " + towersDormantToVazioPerWave[waveNumber]);
                    }

                    // Log Zigurate HP at the end of the wave
                    if (zigurateHpPerWave.ContainsKey(waveNumber))
                    {
                        writer.WriteLine("Zigurate HP at end of wave: " + zigurateHpPerWave[waveNumber]);
                    }

                    writer.WriteLine();
                }

                // If the game ended in GAMEOVER, log the wave it occurred on
                if (gameOverWave != -1)
                {
                    writer.WriteLine("Game ended on Wave: " + gameOverWave);
                }

                writer.WriteLine("Final Moeda: " + finalMoeda);
                writer.WriteLine("Final Zigurate HP: " + finalZigurateHp);
                writer.WriteLine("Stage End.");
                writer.WriteLine("--------------------------------------------------");
            }

            Debug.Log("Data written to file successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error writing data to file: " + ex.Message);
        }
    }

}






