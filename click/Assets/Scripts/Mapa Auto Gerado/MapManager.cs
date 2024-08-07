using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


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
    public GameObject linePrefab;
    public Sprite background;
    public Vector2 nodeArea;
    public Map map;

    private GameObject scrollView;
    private GameObject firstParent;
    private GameObject mapParent;
    private GameObject viewPort;
    private GameObject line;
    private GameManager gameManager;

    private void Start()
    {

        AudioManager.instance.InitializeAmbience(FMODEvents.instance.ambiencia_mapa_level_escolha);

        gameManager = FindFirstObjectByType<GameManager>();

        map = gameManager.GetMap();

        if (map.layers.Count == 0)
            map = MapGenerator(mapConfig);
            gameManager.SetMap(map);

        InstantiateMap(map);

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
                    mapData.layers[iLayers].paths[iPaths].mapLayerIndex = iLayers;
                    mapData.layers[iLayers].paths[iPaths].mapPathIndex = iPaths;
                    mapData.layers[iLayers].paths[iPaths].dif = m.layers[iLayers].dif;

                    randPosNode = false;
                    node = null;                    

                }                

            }

        }

        Debug.Log("Depois dos loops");

        mapData = ConectionPathGenerator(mapData);

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

    public void InstantiateMap(Map m)
    {

        firstParent = new GameObject("Map");
        scrollView = new GameObject("scrollView");
        viewPort = new GameObject("viewPort");
        mapParent = new GameObject("content");

        scrollView.transform.SetParent(firstParent.transform);
        viewPort.transform.SetParent(scrollView.transform);
        mapParent.transform.SetParent(viewPort.transform);

        mapParent.layer = 5;
        mapParent.AddComponent<RectTransform>();
        mapParent.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        mapParent.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        mapParent.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
        mapParent.transform.position = new Vector3(0, 0, 0);
        mapParent.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

        firstParent.layer = 5;
        firstParent.AddComponent<Canvas>();

        viewPort.layer = 5;
        viewPort.AddComponent<CanvasRenderer>();
        viewPort.AddComponent<Mask>();

        scrollView.layer = 5;
        scrollView.AddComponent<ScrollRect>();
        scrollView.AddComponent<CanvasRenderer>();
        scrollView.GetComponent<ScrollRect>().content = mapParent.GetComponent<RectTransform>();
        scrollView.GetComponent<ScrollRect>().vertical = false;
        scrollView.GetComponent<ScrollRect>().horizontal = true;
        scrollView.GetComponent<ScrollRect>().viewport = viewPort.GetComponent<RectTransform>();

        


        for (int iLayers = 0; iLayers < m.layers.Count; iLayers++)
        {

            for(int iPaths = 0; iPaths < m.layers[iLayers].paths.Count; iPaths++)
            {

                if (m.layers[iLayers].paths[iPaths].node != null)
                {

                    if(iLayers == 0)
                    {
                        bool open = true;

                        for(int i = 0; i < m.layers[0].paths.Count; i++)
                        {

                            if(m.layers[0].paths[i].status == STATUSPATH.visited)
                            {

                                open = false;

                            }

                        }

                        if(open) m.layers[iLayers].paths[iPaths].status = STATUSPATH.open;

                    }

                    float x = iLayers * nodeArea.x;
                    float y = iPaths * nodeArea.y * -1;

                    m.layers[iLayers].paths[iPaths].gameObject = InstantiateNode(new Vector2(x, y), m.layers[iLayers].paths[iPaths]) ;
                                        
                }

            }

        }

        for (int iLayers = 0; iLayers < m.layers.Count; iLayers++)
        {

            for (int iPaths = 0; iPaths < m.layers[iLayers].paths.Count; iPaths++)
            {

                if (m.layers[iLayers].paths[iPaths].node != null)
                {
                                        
                    for(int iLines = 0; iLines < m.layers[iLayers].paths[iPaths].iConectPath.Count; iLines++)
                    {

                        float xOrigem = (iLayers * nodeArea.x) + 
                            nodePrefab.transform.GetChild(0).transform.position.x +
                            m.layers[iLayers].paths[iPaths].position.x;

                        float yOrigem = (iPaths * nodeArea.y * -1) + 
                            nodePrefab.transform.GetChild(0).transform.position.y +
                            m.layers[iLayers].paths[iPaths].position.y;

                        float xDestino = ((iLayers+1) * nodeArea.x) + 
                            nodePrefab.transform.GetChild(0).transform.position.x +
                            m.layers[iLayers+1].paths[m.layers[iLayers].paths[iPaths].iConectPath[iLines]].position.x;

                        float yDestino = (m.layers[iLayers].paths[iPaths].iConectPath[iLines] * nodeArea.y * -1) + 
                            nodePrefab.transform.GetChild(0).transform.position.y +
                            m.layers[iLayers + 1].paths[m.layers[iLayers].paths[iPaths].iConectPath[iLines]].position.y;

                        InstantiateLine(new Vector2(xOrigem, yOrigem), new Vector2(xDestino, yDestino));

                        m.layers[iLayers].paths[iPaths].conectPath.Add(line);

                    }           

                }

            }

        }

    }

    public GameObject InstantiateNode(Vector2 xY, Path path)
    {

        GameObject node = Instantiate(nodePrefab, new Vector3(xY.x, xY.y, 0), Quaternion.identity);

        node.GetComponent<NodeManager>().SetPath(path);

        node.transform.SetParent(mapParent.transform);

        return node;

    }

    public void InstantiateLine(Vector2 origem, Vector2 destino)
    {
        line  = Instantiate(linePrefab, new Vector3(origem.x, origem.y, 0), Quaternion.identity);

        line.transform.SetParent(mapParent.transform);

        line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(origem.x, origem.y, 0));
        line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(destino.x, destino.y, 0));

         

    }

    public void MapProgress(Path path)
    {

        for(int iPath = 0; iPath < map.layers[path.mapLayerIndex].paths.Count; iPath++)
        {

            if (map.layers[path.mapLayerIndex].paths[iPath].gameObject != null)
            {

                if(iPath == path.mapPathIndex)
                {

                    map.layers[path.mapLayerIndex].paths[iPath].status = STATUSPATH.visited;
                    map.layers[path.mapLayerIndex].paths[iPath].gameObject.GetComponent<NodeManager>().path.status = STATUSPATH.visited;

                }
                else
                {

                    map.layers[path.mapLayerIndex].paths[iPath].status = STATUSPATH.close;
                    map.layers[path.mapLayerIndex].paths[iPath].gameObject.GetComponent<NodeManager>().path.status = STATUSPATH.close;

                }

            }

        }

        for (int iPath = 0; iPath < map.layers[path.mapLayerIndex].paths.Count; iPath++)
        {

            if (map.layers[path.mapLayerIndex].paths[iPath] != null)
            {

                if (path.iConectPath.Contains(iPath))
                {

                    map.layers[path.mapLayerIndex + 1].paths[iPath].status = STATUSPATH.open;
                    map.layers[path.mapLayerIndex + 1].paths[iPath].gameObject.GetComponent<NodeManager>().path.status = STATUSPATH.open;

                }

            }

        }

        if(path.mapLayerIndex == map.layers.Count-1)
        {

            gameManager.SetUltimaFase(true);

        }

        gameManager.SetMap(map);

    }

}

[Serializable]
public class Path
{

    public GameObject gameObject;
    public STATUSPATH status = STATUSPATH.close;
    public NodeConfig node;
    public Vector2 position;
    public int mapPathIndex;
    public int mapLayerIndex;
    public List<int> iConectPath = new List<int>();
    public List<GameObject> conectPath = new List<GameObject>();
    public DIFICULDADE dif;

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

