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
    public List<TowerList> torres = new List<TowerList>();

    [Header("Inventário")]
    public Inventario[] inventario;

    [Header("Lista de Cenas do Mapa")]
    public List<FaseList> fases = new List<FaseList>();

    [Header("Lista de Reliquias")]
    public List<RelicData> reliquias = new List<RelicData>();

    [Header("Lista de Eventos")]
    public List<EventData> eventos = new List<EventData>();

    [Header("Lista de Tesouros")]
    public List<EventData> tesouros = new List<EventData>();





    void Awake()
    {

        currentHpZigurate = maxHpZigurate;

        inventarioInicial = inventario;

        SetBonusRelic();

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

        for(int i = 0;i < torres.Count; i++)
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

    public void SetRelic(RelicData relic)
    {

        bool add = true;

        for(int iRelic = 0; iRelic < reliquias.Count; iRelic++)
        {

            if(reliquias != null)
            {

                if (reliquias[iRelic].nomeRelic == relic.nomeRelic)
                {

                    add = false;

                }

            }

        }

        if (add)
        {

            reliquias.Add(relic);

            SetBonusRelic();

        }

    }

    public void SetBonusRelic()
    {

        for(int iRelic = 0; iRelic < reliquias.Count; iRelic++)
        {

            if (reliquias[iRelic].tower != "")
            {

                for(int iTower = 0; iTower < torres.Count; iTower++)
                {

                    if (torres[iTower].torres.name == reliquias[iRelic].tower)
                    {

                        bool add = true;

                        for(int iRecurso = 0; iRecurso < torres[iTower].torres.tipoRecurso.Count; iRecurso++)
                        {

                            if (torres[iTower].torres.tipoRecurso[iRecurso] == reliquias[iRelic].addTipoRecurso)
                            {

                                add = false;

                            }

                        }

                        if(add)
                        {

                            torres[iTower].torres.tipoRecurso.Add(reliquias[iRelic].addTipoRecurso);

                        }

                    }

                }

            }
            else
            {

                for (int iTower = 0; iTower < torres.Count; iTower++)
                {

                    if (torres[iTower].torres.tipoRecurso[0] == reliquias[iRelic].tipoRecurso)
                    {

                        bool add = true;

                        for (int iRecurso = 0; iRecurso < torres[iTower].torres.tipoRecurso.Count; iRecurso++)
                        {

                            if (torres[iTower].torres.tipoRecurso[iRecurso] == reliquias[iRelic].addTipoRecurso)
                            {

                                add = false;

                            }

                        }

                        if (add)
                        {

                            torres[iTower].torres.tipoRecurso.Add(reliquias[iRelic].addTipoRecurso);

                        }

                    }

                }

            }

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
public class FaseList
{

    public int indexScene;
    public NODETYPE nodeType;
    public DIFICULDADE dif;

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