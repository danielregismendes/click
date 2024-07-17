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
    public TOWERSTATE state = TOWERSTATE.VAZIO;
    public TowerData towerData = null;
    public bool viewAreaAtk = false;

    private GameManager gameManager;
    private Attack attack;
    private UIManager uiManager;


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
                    }

                    towerMenu.GetComponent<BtTower>().SetPlaceTower(gameObject);

                }

                break;

            case TOWERSTATE.CONSTRUIDO:
                break;

        }

    }

    public void Select(bool toggle)
    {

        select.SetActive(toggle);

    }


    public void BuildingTower(string nameTower)
    {

        string tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso;
        int qtdRecurso = gameManager.GetInventario(tipoRecurso);
        int custoTorre = gameManager.GetTower(nameTower).qtdRecurso;

        if (qtdRecurso >= custoTorre)
        {

            towerMenu.SetActive(false);
            gameManager.SetInventario(tipoRecurso, custoTorre * -1);
            uiManager.AtualizarUI();
            state = TOWERSTATE.CONSTRUIDO;
            towerData = gameManager.GetTower(nameTower);
            tower = Instantiate(towerData.gameModelTower, tower.transform.position, tower.transform.rotation);
            tower.SetActive(true);
            troopArea.SetActive(true);
            troopArea.GetComponent<SphereCollider>().radius = towerData.atkRaio;
            DrawAreaAtk();
            attack.SetAtk(towerData.atkSpeed, towerData.atkDamage);
            Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(0).position, troopArea.transform.GetChild(0).rotation);
            Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(1).position, troopArea.transform.GetChild(0).rotation);
            Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(2).position, troopArea.transform.GetChild(0).rotation);


        }

    }

    private void DrawAreaAtk()
    {        
            
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(troopArea.transform.position, towerData.atkRaio);            

    }

}
