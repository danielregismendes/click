using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Tower")]
public class TowerData : ScriptableObject
{
    [Header("Parametros Gerais")]
    public string towerName;
    public string towerDescription;
    public GameObject gameModelTower;
    public GameObject gameModelTroop;

    [Header("Parametros de Ataque")]
    public float atkSpeed;
    public int atkDamage;
    [Range(0.01f, 100.00f)]
    public float atkRaio;
    public AttackType attackType; // Add this to specify attack type
    public float slowPercentage;  // Add this to define the slowdown effect for each tower


    [Header("Parametros de Construção")]
    public List<string> tipoRecurso = new List<string>();
    public int qtdRecurso;
}
