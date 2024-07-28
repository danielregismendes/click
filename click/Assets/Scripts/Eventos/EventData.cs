using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Evento")]
public class EventData : ScriptableObject
{

    [Header("Descrição do Evento")]
    public string nomeEvento;
    [TextArea(4, 10)]
    public string descEvento;
    public Sprite background;

    [Header("Opções")]
    public List<Options> options = new List<Options>();
}

[Serializable]
public class Options
{

    [TextArea(2, 10)]
    public string txtOption;

    [Header("Bonus da Escolha")]
    public RelicData reliquia;
    public string tipoRecurso;
    public int qtdRecurso;
    public int vidaZigurate;

}