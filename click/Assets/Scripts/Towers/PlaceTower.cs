using System.Collections;
using System.Collections.Generic;
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

    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
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
        Debug.Log(nameTower);

        string tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso;
        int qtdRecurso = gameManager.GetInventario(tipoRecurso);
        int custoTorre = gameManager.GetTower(nameTower).qtdRecurso;

        if (qtdRecurso >= custoTorre)
        {

            towerMenu.SetActive(false);
            gameManager.SetInventario(tipoRecurso, custoTorre * -1);
            state = TOWERSTATE.CONSTRUIDO;
            towerData = gameManager.GetTower(nameTower);
            tower = Instantiate(towerData.gameModelTower, tower.transform.position, tower.transform.rotation);
            tower.SetActive(true);

        }

    }

}
