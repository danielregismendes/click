using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtTower : MonoBehaviour
{

    private PlaceTower placeTower;

    public void SetTower(string nameTower)
    {

        if (placeTower) placeTower.BuildingTower(nameTower);

    }

    public void SetPlaceTower(GameObject placeTowerSet)
    {

        placeTower = placeTowerSet.GetComponent<PlaceTower>();

    }

}
