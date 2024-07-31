using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NODETYPE
{
    VilaVermelhaAzul,
    VilaVermelhaAmarelo,
    VilaAmareloAzul,
    Tesouro,
    Evento,
    Boss
}

[CreateAssetMenu]
public class NodeConfig : ScriptableObject
{

    public Sprite sprite;
    public NODETYPE nodeType;

}
