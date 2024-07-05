using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tower")]
public class PlantData : ScriptableObject
{

    [Header("Tower Settings")]
    public string towerName;
    public string towerDescription;
    public GameObject gameModelTower;
    public GameObject gameModelTroop;
    public float atkSpeed;
    public float atkDamage;

}