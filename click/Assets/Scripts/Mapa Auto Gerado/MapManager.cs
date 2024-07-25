using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum MapOrientation
{

    LeftToRight,
    UpToDown,
    RightToLeft,
    DownToUp

}

public class MapManager : MonoBehaviour
{

    public MapOrientation orientation;
    public MapConfig mapConfig;
    public GameObject nodePrefab;
    public Sprite background;
    public Map map;

    private void Start()
    {

        Debug.Log("Antes do processo");

        map = MapGenerator(mapConfig);

        Debug.Log(map);

        map = ConectionPathGenerator(map);

    }


    public Map MapGenerator(MapConfig m)
    {
        Map mapData = new Map();
        LayerMapData node = null;
        int qtdNodes = 0;
        int randInt = 0;
        List<int> indiceNodes = new List<int>();
        bool randPosNode = false; 
        float xPosition = 0f;
        float yPosition = 0f;

        Debug.Log("Antes dos loops");

        for (int iLayers = 0; iLayers < m.layers.Length; iLayers++)
        {

            Debug.Log("Loop 1");

            mapData.layers.Add(new Layer());

            indiceNodes.Clear();

            qtdNodes = GerarRandomInt(m.layers[iLayers].minNodes, m.layers[iLayers].maxNodes);

            for(int iRandom = 0; iRandom < qtdNodes;)
            {

                Debug.Log("Loop 2");

                
                randInt = GerarRandomInt(1, m.maxPaths);

                while (indiceNodes.Contains(randInt))
                {

                    randInt = GerarRandomInt(1, m.maxPaths);

                }

                indiceNodes.Add(randInt);
                iRandom++;

            }            

            for (int iPaths = 0; iPaths < m.maxPaths; iPaths++)
            {

                Debug.Log("Loop 3");

                mapData.layers[iLayers].paths.Add(new Path());

                if (indiceNodes.Contains(iPaths))
                {

                    while(!randPosNode)
                    {

                        Debug.Log("Loop 4");


                        int rand = GerarRandomInt(1, 100);

                        for(int i = 0; i < m.layers[iLayers].layerData.Length; i++)
                        {

                            Debug.Log("Loop 5");

                            if (m.layers[iLayers].layerData[i].spawnChance > rand)
                            {

                                if(i != 0 && node != null)
                                {
                                    
                                    if(node.spawnChance >= m.layers[iLayers].layerData[i].spawnChance)
                                    {

                                        node = m.layers[iLayers].layerData[i];

                                    }
                                    
                                }
                                else
                                {

                                    node = m.layers[iLayers].layerData[i];

                                }

                            }

                        }

                        if (node != null) randPosNode = true;

                    }

                    

                    xPosition = GerarRandomFloat(m.layers[iLayers].randomPosition * -1, m.layers[iLayers].randomPosition);
                    yPosition = GerarRandomFloat(m.layers[iLayers].randomPosition * -1, m.layers[iLayers].randomPosition);

                    mapData.layers[iLayers].paths[iPaths].node = node.node;
                    mapData.layers[iLayers].paths[iPaths].position = new Vector2(xPosition, yPosition);

                    randPosNode = false;
                    node = null;                    

                }                

            }

        }

        Debug.Log("Depois dos loops");

        return mapData;

    }

    public Map ConectionPathGenerator(Map m)
    {
        int qtdPaths = 0;
        int qtdPathsForward = 0;
        List<int> PathsForward = new List<int>();
        List<int> Paths = new List<int>();
        int qtdConetionPathTotal = 0;
        int qtdConetionPath = 0;
        int qtdConetionPathAdd = 0;
        int pathSobra = 0;

        for (int iLayers = 0; iLayers < m.layers.Count; iLayers++)
        {



            for(int iPaths = 0; iPaths < m.layers[iLayers].paths.Count; iPaths++)
            {

                if(m.layers[iLayers].paths[iPaths].node != null)
                {

                    qtdPaths++;
                    Paths.Add(iPaths);

                }

            }

            if (qtdPaths == 1)
            {

                for (int iPaths = 0; iPaths < m.layers[iLayers].paths.Count; iPaths++)
                {

                    if (m.layers[iLayers].paths[iPaths].node != null && iLayers + 1 < m.layers.Count)
                    {

                        for (int iPathsForward = 0; iPathsForward < m.layers[iLayers + 1].paths.Count; iPathsForward++)
                        {

                            if (m.layers[iLayers + 1].paths[iPathsForward].node != null)
                            {

                                m.layers[iLayers].paths[iPaths].iConectPath.Add(iPathsForward);

                            }

                        }

                    }

                }

            }
            else
            {

                pathSobra = GerarRandomInt(0, qtdPaths - 1);                

                for (int iPaths = 0; iPaths < m.layers[iLayers].paths.Count; iPaths++)
                {

                    if (m.layers[iLayers].paths[iPaths].node != null && iLayers + 1 < m.layers.Count)
                    {

                        for (int iPathsForward = 0; iPathsForward < m.layers[iLayers + 1].paths.Count; iPathsForward++)
                        {

                            if (m.layers[iLayers + 1].paths[iPathsForward].node != null)
                            {

                                qtdPathsForward++;
                                PathsForward.Add(iPathsForward);

                            }

                        }

                        if(PathsForward.Count > qtdPaths)
                        {

                            qtdConetionPathTotal = PathsForward.Count;

                        }
                        else
                        {

                            qtdConetionPathTotal = qtdPaths;

                        }

                        qtdConetionPath = (qtdConetionPathTotal - (qtdConetionPathTotal % qtdPaths)) / qtdPaths;

                        if (Paths[pathSobra] == iPaths)
                        {
                            qtdConetionPath = qtdConetionPath + (qtdConetionPathTotal % qtdPaths);
                        }

                        for (int iQtdPaths = 0; iQtdPaths < qtdConetionPath; iQtdPaths++)
                        {

                            if(iQtdPaths + qtdConetionPathAdd < PathsForward.Count)
                            {

                                m.layers[iLayers].paths[iPaths].iConectPath.Add(PathsForward[iQtdPaths + qtdConetionPathAdd]);
                                qtdConetionPathAdd++;

                            }
                            else
                            {

                                m.layers[iLayers].paths[iPaths].iConectPath.Add(PathsForward[PathsForward.Count-1]);

                            }


                        }                        

                    }

                    PathsForward.Clear();

                }

                qtdConetionPathAdd = 0;

            }

            qtdPaths = 0;            
            Paths.Clear();

        }

        return m;

    }

    public int GerarRandomInt(int min, int max)
    {

        return UnityEngine.Random.Range(min, max);

    }

    public float GerarRandomFloat(float min, float max)
    {

        return UnityEngine.Random.Range(min, max);

    }

}

[Serializable]
public class Path
{

    public NodeConfig node;
    public Vector2 position;
    public List<int> iConectPath = new List<int>();

}


[Serializable]
public class Layer
{

    public List<Path> paths = new List<Path>();

}

[Serializable]
public class Map
{

    public List<Layer> layers = new List<Layer>();

}

