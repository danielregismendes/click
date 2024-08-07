using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum STAGEFASE
{
    MENU,
    RUN,
    GAMEOVER

}

public class GameManager : MonoBehaviour
{

    private GameManager gameManager;

    public int maxHpZigurate;
    private int currentHpZigurate;
    private Inventario[] inventarioInicial;
    public STAGEFASE stage = STAGEFASE.MENU;
    [SerializeField] private Map currentMap;
    private EventData currentEvent;
    private bool ultimaFase;

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

        UIManager animCanvas;

        animCanvas = FindFirstObjectByType<UIManager>();            

        if (stage != STAGEFASE.GAMEOVER && currentHpZigurate - damage <= 0)
        {

            stage = STAGEFASE.GAMEOVER;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.vinheta_derrota, Camera.main.transform.position);
            if (animCanvas) animCanvas.GetComponent<Animator>().SetTrigger("Game Over");

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

            if(reliquias.Count > 0)
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

    public void SetMap(Map m)
    {

        currentMap = m;

    }

    public Map GetMap()
    {

        return currentMap;

    }

    public int GerarRandomInt(int min, int max)
    {

        return UnityEngine.Random.Range(min, max);

    }

    public EventData GetCurrentEventData()
    {

        return currentEvent;

    }

    public void LoadFase(Path path)
    {

        List<int> listFases = new List<int>();
        int randEvent = 0;
        int randFase = 0;

        for(int iFases = 0; iFases < fases.Count; iFases++)
        {

            if(path.node.nodeType == fases[iFases].nodeType && path.dif == fases[iFases].dif)
            {

                listFases.Add(fases[iFases].indexScene);

            }

        }

        randFase = GerarRandomInt(0, listFases.Count);

        switch (path.node.nodeType)
        {

            case NODETYPE.Tesouro:

                randEvent = GerarRandomInt(0, tesouros.Count);
                currentEvent = tesouros[randEvent];
                SceneManager.LoadScene(listFases[randFase]);
                listFases.Clear();
                break;

            case NODETYPE.Evento:

                randEvent = GerarRandomInt(0, eventos.Count);
                currentEvent = eventos[randEvent];
                SceneManager.LoadScene(listFases[randFase]);
                listFases.Clear();
                break;

            default:
                Debug.Log("Rand " + randFase);
                Debug.Log("Lista " + listFases.Count);
                Debug.Log("Fase " + listFases[randFase]);
                SceneManager.LoadScene(listFases[randFase]);
                listFases.Clear();
                break;

        }

    }

    public void SetUltimaFase(bool ultimaFase)
    {

        this.ultimaFase = ultimaFase;

    }

    public bool GetUltimaFase()
    {

        return ultimaFase;

    }

    public void GameOver()
    {

        inventario = inventarioInicial;
        SceneManager.LoadScene(0);
        Destroy(gameObject);

    }

    public void Win()
    {

        SceneManager.LoadScene(2);

    }

    public void ZerarGame()
    {

        SceneManager.LoadScene(1);
        Destroy(gameObject);

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