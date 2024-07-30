using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Relic")]
public class RelicData : ScriptableObject
{

    [Header("Descrição Reliquia")]
    public string nomeRelic;
    public Sprite imgRelic;
    public string descBonusRelic;

    
    [Header("Aplicar em:")]
    [Tooltip("Aplica as mudanças ao tipo de torre selecionada")]
    public string tower;
    [Tooltip("Aplica as mudanças nas torres ou inimigos que usam o recurso selecionado")]
    public string tipoRecurso;

    [Header("Bonus de Reliquia")]
    [Tooltip("Soma o valor a variavel de damage da torre")]
    public int towerAtkDamage;
    [Tooltip("Soma o valor a variavel de atkspeed da torre")]
    public float towerAtkSpeed;
    [Tooltip("Soma o valor a variavel de custo da torre")]
    public int custTower;
    [Tooltip("Soma o valor a variavel de drop de recurso")]
    public int drop;
    [Tooltip("Libera construção com o tipo de recurso a seguir")]
    public int addTipoRecurso;


}
