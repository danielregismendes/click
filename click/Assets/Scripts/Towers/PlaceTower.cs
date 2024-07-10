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
        string tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso;
        int qtdRecurso = gameManager.GetInventario(tipoRecurso);
        int custoTorre = gameManager.GetTower(nameTower).qtdRecurso;

        if (qtdRecurso >= custoTorre)
        {

            gameManager.SetInventario(tipoRecurso, custoTorre * -1);
            state = TOWERSTATE.CONSTRUIDO;
            towerData = gameManager.GetTower(nameTower);
            tower = Instantiate(towerData.gameModelTower, tower.transform.position, tower.transform.rotation);
            tower.SetActive(true);

        }

    }


    /*
    public void Seeding(string plantName)
    {
        int qtdSeed = gameManager.GetSeeds(plantName);

        if (qtdSeed > 0)
        {
            gameManager.SetInventario(plantName, 0, -1);
            state = FARMSTATE.PLANTADO;
            plantData = gameManager.GetPlantData(plantName);
            plant = Instantiate(plantData.gameModelPlant, plant.transform.position, plant.transform.rotation);
            plant.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
            plant.SetActive(true);
            initialTime = gameManager.gameTimer;

        }
    }

    void Growing()
    {
        float crescimento = (gameManager.gameTimer - initialTime) / plantData.growTime;

        if (crescimento <= 1)
        {
            plant.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100 - Mathf.Round(crescimento * 100));

        }
        else
        {
            state = FARMSTATE.COLHEITA;
        }

    }
    */
}
