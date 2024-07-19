using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum STAGEFASE
{
    MENU,
    FASE,
    CLEARFASE,
    GAMEOVER,
}

public class GameManager : MonoBehaviour
{

    private GameManager gameManager;

    public int maxHpZigurate;
    private int currentHpZigurate;
    private Inventario[] inventarioInicial;
    private STAGEFASE stage = STAGEFASE.MENU;

    [Header("Lista de Torres")]
    public TowerList[] torres;

    [Header("Invent�rio")]
    public Inventario[] inventario;


    void Awake()
    {

        currentHpZigurate = maxHpZigurate;

        inventarioInicial = inventario;

        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    public void SetInventario(string inventarioName, int qtd)
    {

        for (int i = 0; i < inventario.Length; i++)
        {
            if (inventarioName == inventario[i].nome)
            {
                inventario[i].qtd += qtd;
                return;
            }
        }

    }

    public int GetInventario(string inventarioName)
    {

        for (int i = 0; i < inventario.Length; i++)
        {
            if (inventarioName == inventario[i].nome)
            {
                return (inventario[i].qtd);

            }
        }

        return 0;

    }

    public TowerData GetTower(string towerName)
    {

        for(int i = 0;i < torres.Length; i++)
        {

            if(towerName == torres[i].torres.towerName)
            {
                return torres[i].torres;
            }

        }

        return null;

    }

    public int GetHpZigurate()
    {

        return currentHpZigurate;

    }

    public void SetHpZigurate(int damage)
    {

        Animator animCanvas;

        animCanvas = FindFirstObjectByType<UIManager>().GetComponent<Animator>();

        if (stage != STAGEFASE.GAMEOVER && currentHpZigurate - damage <= 0)
        {

            stage = STAGEFASE.GAMEOVER;
            animCanvas.SetTrigger("Game Over");

        }
        else
        {

            currentHpZigurate -= damage;

        }

    }

    public void GameOver()
    {

        inventario = inventarioInicial;
        SceneManager.LoadScene(0);

    }

    public void Win()
    {

        inventario = inventarioInicial;
        SceneManager.LoadScene(0);

    }

}

[Serializable]
public class Inventario
{

    public string nome;
    public int qtd;

}

[Serializable]
public class TowerList
{

    public TowerData torres;

}