using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Towers")]
public class PlantData : ScriptableObject
{

    [Header("Tower Settings")]
    public string towerName;
    public string towerDescription;
    public GameObject gameModelTower;
    public GameObject gameModelAtk;
    public GameObject gameModelAreaAtk;

    [Header("Attack Configuration")]
    public float atkSpeed;
    public float atkDamage;
    public float atkDistance;
    public bool Area;
    public float areaAtk;

}