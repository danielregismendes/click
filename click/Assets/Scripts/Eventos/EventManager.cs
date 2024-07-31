using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventoManager : MonoBehaviour
{

    public EventData eventData;
    public GameObject nmEventoObj;
    public GameObject txtEventoObj;
    public GameObject imgEventoObj;
    public GameObject bt1;
    public GameObject bt2;
    public GameObject bt3;
    public GameObject bt4;
    public GameObject bt5;

    private GameManager gameManager;

    private void Start()
    {

        gameManager = FindFirstObjectByType<GameManager>();

        SetEvent(gameManager.GetCurrentEventData());

        AtualizarUI();

    }

    public void SetEvent(EventData eventData)
    {

        this.eventData = eventData;

    }

    public void AtualizarUI()
    {

        nmEventoObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.nomeEvento;
        txtEventoObj.GetComponent<TextMeshProUGUI>().text =eventData.descEvento;
        imgEventoObj.GetComponent<Image>().sprite = eventData.background;

        bt1.SetActive(false);
        bt2.SetActive(false);
        bt3.SetActive(false);
        bt4.SetActive(false);
        bt5.SetActive(false);

        for (int iOptions = 0; iOptions < eventData.options.Count; iOptions++)
        {

            switch(iOptions)
            {

                case 0:

                    bt1.SetActive(true);
                    bt1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.options[iOptions].txtOption;
                    break;

                case 1:

                    bt2.SetActive(true);
                    bt2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.options[iOptions].txtOption;
                    break;

                case 2:

                    bt3.SetActive(true);
                    bt3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.options[iOptions].txtOption;
                    break;

                case 3:

                    bt4.SetActive(true);
                    bt4.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.options[iOptions].txtOption;
                    break;

                case 4:

                    bt5.SetActive(true);
                    bt5.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = eventData.options[iOptions].txtOption;
                    break;

            }

        }

    }

    public void SetBonusEvento(int iOption)
    {

        if(eventData.options[iOption].reliquia != null)
            gameManager.SetRelic(eventData.options[iOption].reliquia);

        if (eventData.options[iOption].tipoRecurso != null)
            gameManager.SetInventario(eventData.options[iOption].tipoRecurso, eventData.options[iOption].qtdRecurso);

        if (eventData.options[iOption].vidaZigurate != 0)
            gameManager.SetHpZigurate(eventData.options[iOption].vidaZigurate);

        SceneManager.LoadScene(2);

    }

}
