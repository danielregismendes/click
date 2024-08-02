using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Musicas")]
    [field: SerializeField] public EventReference music_eventos { get; private set; }
    [field: SerializeField] public EventReference music_gameplay { get; private set; }

    [field: Header("Ambientação")]
    [field: SerializeField] public EventReference ambiencia_gamplay { get; private set; }
    [field: SerializeField] public EventReference ambiencia_mapa_level_escolha { get; private set; }

    [field: Header("Gameplay SFX")]
    [field: SerializeField] public EventReference coin_points { get; private set; }
    [field: SerializeField] public EventReference colocar_torre { get; private set; }
    [field: SerializeField] public EventReference compratorres { get; private set; }
    [field: SerializeField] public EventReference inicio_de_orda { get; private set; }
    [field: SerializeField] public EventReference inimigo_dano { get; private set; }
    [field: SerializeField] public EventReference inimigo_morte { get; private set; }
    [field: SerializeField] public EventReference inimigo_morte_contatoaosolo { get; private set; }
    [field: SerializeField] public EventReference inimigo_steps { get; private set; }
    [field: SerializeField] public EventReference litch_dano { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference ui_click_main_menu_avanco { get; private set; }
    [field: SerializeField] public EventReference ui_click_main_menu_estatico { get; private set; }
    [field: SerializeField] public EventReference ui_click_main_menu_hover { get; private set; }
    [field: SerializeField] public EventReference ui_click_main_menu_retorno { get; private set; }
    [field: SerializeField] public EventReference ui_click_main_menu_startgame { get; private set; }
    [field: SerializeField] public EventReference ui_click_submenu_menu { get; private set; }

    [field: Header("Vinhetas")]
    [field: SerializeField] public EventReference vinheta_derrota { get; private set; }
    [field: SerializeField] public EventReference vinheta_vitoria_jogo { get; private set; }
    [field: SerializeField] public EventReference vinheta_vitoriaestagio { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}