using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    public float maxDistance = 500;

    PlaceTower placeTower;

    private bool selectTower = true;

    private void Update()
    {

        if (selectTower)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (placeTower != null)
                {
                    if (placeTower.transform != hit.transform)
                    {
                        placeTower.Select(false);

                    }
                }

                placeTower = hit.transform.GetComponent<PlaceTower>();
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

    public void SetSelectTower(bool select)
    {

        selectTower = select;

    }

}
