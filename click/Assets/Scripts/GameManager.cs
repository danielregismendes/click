using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private GameManager gameManager;

    public int maxHpZigurate;
    private int currentHpZigurate;

    [Header("Lista de Torres")]
    public TowerList[] torres;

    [Header("Invent�rio")]
    public Inventario[] inventario;


    void Awake()
    {

        currentHpZigurate = maxHpZigurate;

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

        currentHpZigurate -= damage;

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