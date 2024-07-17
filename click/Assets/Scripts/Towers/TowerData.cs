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

    [Header("Parametros de Construção")]
    public string tipoRecurso;
    public int qtdRecurso;

}