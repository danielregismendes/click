using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TOWERSTATE
{
    VAZIO,
    CONSTRUIDO,
}

public class PlaceTower : MonoBehaviour
{

    public GameObject select;
    public GameObject tower;
    public GameObject troopArea;
    public GameObject towerMenu;
    public GameObject destroyMenu;
    public TOWERSTATE state = TOWERSTATE.VAZIO;
    public TowerData towerData = null;
    public bool viewAreaAtk = false;

    private GameManager gameManager;
    private Attack attack;
    private UIManager uiManager;

    private GameObject t1;
    private GameObject t2;
    private GameObject t3;


    private void Start()
    {

        gameManager = FindFirstObjectByType<GameManager>();
        attack = troopArea.GetComponent<Attack>();
        uiManager = FindFirstObjectByType<UIManager>();
    }

    void Update()
    {

        switch (state)

        {           

            case TOWERSTATE.VAZIO:

                if (select.active & Input.GetMouseButtonDown(0))
                {
                    if (!towerMenu.active)
                    {
                        towerMenu.SetActive(true);
                        FindAnyObjectByType<MouseSelect>().SetSelectTower(false);
                    }

                    towerMenu.GetComponent<BtTower>().SetPlaceTower(gameObject);

                }

                break;

            case TOWERSTATE.CONSTRUIDO:

                if (select.active & Input.GetMouseButtonDown(0))
                {
                    if (!destroyMenu.active)
                    {
                        destroyMenu.SetActive(true);
                        destroyMenu.GetComponent<BtTower>().SetPlaceTower(gameObject);
                        FindAnyObjectByType<MouseSelect>().SetSelectTower(false);
                    }

                }

                break;

        }

    }

    public void Select(bool toggle)
    {

        select.SetActive(toggle);

    }

    public void DestroyTower()
    {
        
        Destroy(t1);
        Destroy(t2);
        Destroy(t3);
        tower.SetActive(false);
        troopArea.SetActive(false);
        state = TOWERSTATE.VAZIO;
    }

    public void BuildingTower(string nameTower)
    {

        string tipoRecurso = null;        
        int custoTorre = gameManager.GetTower(nameTower).qtdRecurso;

        for(int iRec = 0; iRec < gameManager.GetTower(nameTower).tipoRecurso.Count; iRec++)
        {

            if(gameManager.GetTower(nameTower).tipoRecurso.Count == 1)
            {

                tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[iRec];

            }
            else
            {

                if (gameManager.GetInventario(gameManager.GetTower(nameTower).tipoRecurso[iRec]) >= custoTorre)
                {

                    tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[iRec];
                    break;

                }

            }

        }

        if(tipoRecurso == null) tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[0];

        int qtdRecurso = gameManager.GetInventario(tipoRecurso);

        if (qtdRecurso >= custoTorre)
        {

            towerMenu.SetActive(false);
            FindAnyObjectByType<MouseSelect>().SetSelectTower(true);
            gameManager.SetInventario(tipoRecurso, custoTorre * -1);
            uiManager.AtualizarUI();
            state = TOWERSTATE.CONSTRUIDO;
            towerData = gameManager.GetTower(nameTower);
            tower = Instantiate(towerData.gameModelTower, tower.transform.position, tower.transform.rotation);
            tower.SetActive(true);
            troopArea.SetActive(true);
            troopArea.GetComponent<SphereCollider>().radius = towerData.atkRaio;
            //DrawAreaAtk();
            attack.SetAtk(towerData.atkSpeed, towerData.atkDamage);
            t1 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(0).position, troopArea.transform.GetChild(0).rotation);
            t2 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(1).position, troopArea.transform.GetChild(0).rotation);
            t3 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(2).position, troopArea.transform.GetChild(0).rotation);


        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(viewAreaAtk)
        {

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(troopArea.transform.position, towerData.atkRaio);

        }         

    }
#endif
}
