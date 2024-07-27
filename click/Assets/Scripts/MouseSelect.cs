using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scenes
{

    mainMenu,
    mapMenu,
    sceneFase

}

public class MouseSelect : MonoBehaviour
{
    public Scenes scene;
    public float maxDistance = 500;

    private PlaceTower placeTower;
    private NodeManager nodeManager;
    private bool selectTower = true;

    private void Update()
    {

        switch (scene)
        {

            case Scenes.mainMenu:
                break;

            case Scenes.mapMenu:
                SelectMapNodes();
                break;

            case Scenes.sceneFase:
                SelectTower();
                break;

        }              

    }

    public void SelectTower()
    {

        if (selectTower)
        {

            if (GetMouseObject() != null)
            {

                if (placeTower != null)
                {
                    if (placeTower.transform != GetMouseObject().transform)
                    {

                        placeTower.Select(false);

                    }
                }

                placeTower = GetMouseObject().GetComponent<PlaceTower>();

                if (placeTower != null)
                {
                    placeTower.Select(true);
                }
                else
                {
                    if (placeTower != null)
                    {

                        placeTower.Select(false);

                    }

                    placeTower = null;

                }


            }
            else if (placeTower != null)
            {

                placeTower.Select(false);

            }

        }

    }

    public void SelectMapNodes()
    {

        if (GetMouseObject() != null)
        {

            if (nodeManager != null)
            {
                if (nodeManager.transform != GetMouseObject().transform)
                {

                    nodeManager.Select(false);

                }
            }

            nodeManager = GetMouseObject().GetComponent<NodeManager>();

            if (nodeManager != null)
            {
                nodeManager.Select(true);
            }
            else
            {
                if (nodeManager != null)
                {

                    nodeManager.Select(false);

                }

                nodeManager = null;

            }


        }
        else if (nodeManager != null)
        {

            nodeManager.Select(false);

        }

    }

    public GameObject GetMouseObject()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject obj = null;
        
        if(Physics.Raycast(ray, out hit, maxDistance))
        {

            obj = hit.transform.gameObject;

        }

        return obj;

    }

    public void SetSelectTower(bool select)
    {

        selectTower = select;

    }

}
