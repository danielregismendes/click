using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MapConfig : ScriptableObject
{

    public int maxPaths;
    public LayerMap[] layers;

}

[Serializable]
public class LayerMap
{

    public LayerMapData[] layerData;
    [Tooltip("O valor muda a posição aleatoriamente nos " +
        "eixos x e y positiva ou negativamente, respeitando o limite da variável.")]
    [Range(0.00f, 1.90f)]
    public float randomPosition;
    [Tooltip("A quantidade de nodes deve ser inferior ou igual valor da variavel maxNodes.")]
    public int minNodes;
    [Tooltip("A quantidade de nodes deve ser no maximo o valor da variavel maxPaths.")]
    public int maxNodes;

}

[Serializable]
public class LayerMapData
{

    public NodeConfig node;
    [Tooltip("A soma de todos spawnChance da mesma layer deve ser 100.")]
    [Range(0.01f, 100.00f)]
    public float spawnChance;

}
