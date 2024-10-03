using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TOWERSTATE
{
    VAZIO,
    CONSTRUIDO,
    DORMENTE,
}

public class PlaceTower : MonoBehaviour
{

    public GameObject select;
    public GameObject tower;
    public GameObject troopArea;
    public GameObject towerMenu;
    public GameObject destroyMenu;
    public TOWERSTATE state = TOWERSTATE.VAZIO;
    public TowerData towerData = null;
    public bool viewAreaAtk = false;
    private float lastClickTime = 0f; // Tracks the last click time
    private float doubleClickThreshold = 0.3f; // Time threshold for double click in seconds
    public int moedaCostToActivate = 20; // Cost in "moeda" to go from DORMENTE to VAZIO


    private GameManager gameManager;
    private Attack attack;
    private UIManager uiManager;

    private GameObject t1;
    private GameObject t2;
    private GameObject t3;
    
    private int bonusAtk = 0;
    private float bonusAtkSpeed = 0;
    private int bonusCustTower = 0;


    private void Start()
    {

        gameManager = FindFirstObjectByType<GameManager>();
        attack = troopArea.GetComponent<Attack>();
        uiManager = FindFirstObjectByType<UIManager>();

    }

    void Update()
    {
        switch (state)
        {
            case TOWERSTATE.DORMENTE:
                if (select.activeSelf && Input.GetMouseButtonDown(0))
                {
                    float timeSinceLastClick = Time.time - lastClickTime;

                    if (timeSinceLastClick <= doubleClickThreshold)
                    {
                        // It's a double-click, attempt to transition to VAZIO
                        AttemptToActivateSlot();
                    }

                    lastClickTime = Time.time;
                }
                break;

            case TOWERSTATE.VAZIO:
                if (select.activeSelf && Input.GetMouseButtonDown(0))
                {
                    if (!towerMenu.activeSelf)
                    {
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.compratorres, Camera.main.transform.position);
                        towerMenu.SetActive(true);
                        FindAnyObjectByType<MouseSelect>().SetSelectTower(false);
                    }

                    towerMenu.GetComponent<BtTower>().SetPlaceTower(gameObject);
                }
                break;

            case TOWERSTATE.CONSTRUIDO:
                if (select.activeSelf && Input.GetMouseButtonDown(0))
                {
                    if (!destroyMenu.activeSelf)
                    {
                        destroyMenu.SetActive(true);
                        destroyMenu.GetComponent<BtTower>().SetPlaceTower(gameObject);
                        FindAnyObjectByType<MouseSelect>().SetSelectTower(false);
                    }
                }
                break;
        }
    }

    public void Select(bool toggle)
    {
               
        select.SetActive(toggle);

    }

    private void AttemptToActivateSlot()
    {
        int currentMoeda = gameManager.GetInventario("Moeda");

        // Debug to check the current moeda value
        Debug.Log("Current moeda available: " + currentMoeda);
        Debug.Log("Moeda required to activate: " + moedaCostToActivate);

        if (currentMoeda >= moedaCostToActivate)
        {
            // Deduct the required "moeda" to activate the slot
            gameManager.SetInventario("Moeda", -moedaCostToActivate);

            // Verify if the moeda was successfully deducted
            currentMoeda = gameManager.GetInventario("Moeda");
            Debug.Log("Moeda after deduction: " + currentMoeda);

            uiManager.AtualizarUI(); // Update the UI to reflect the new resource count

            // Transition the state to VAZIO
            state = TOWERSTATE.VAZIO;
            Debug.Log("Slot activated. State changed to VAZIO.");
        }
        else
        {
            // Notify the player that they don't have enough "moeda"
            Debug.Log("Not enough 'moeda' to activate this slot.");
        }
    }

    public void DestroyTower()
    {
        
        Destroy(t1);
        Destroy(t2);
        Destroy(t3);
        tower.SetActive(false);
        troopArea.SetActive(false);
        state = TOWERSTATE.VAZIO;
    }

    public void BuildingTower(string nameTower)
    {
        SetBonusRelic(nameTower);

        string tipoRecurso = null;
        int custoTorre = gameManager.GetTower(nameTower).qtdRecurso + bonusCustTower;

        for (int iRec = 0; iRec < gameManager.GetTower(nameTower).tipoRecurso.Count; iRec++)
        {
            if (gameManager.GetTower(nameTower).tipoRecurso.Count == 1)
            {
                tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[iRec];
            }
            else
            {
                if (gameManager.GetInventario(gameManager.GetTower(nameTower).tipoRecurso[iRec]) >= custoTorre)
                {
                    tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[iRec];
                    break;
                }
            }
        }

        if (tipoRecurso == null) tipoRecurso = gameManager.GetTower(nameTower).tipoRecurso[0];
        int qtdRecurso = gameManager.GetInventario(tipoRecurso);

        if (qtdRecurso >= custoTorre)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.colocar_torre, Camera.main.transform.position);
            towerMenu.SetActive(false);
            FindAnyObjectByType<MouseSelect>().SetSelectTower(true);
            gameManager.SetInventario(tipoRecurso, custoTorre * -1);
            uiManager.AtualizarUI();
            state = TOWERSTATE.CONSTRUIDO;
            towerData = gameManager.GetTower(nameTower);
            tower = Instantiate(towerData.gameModelTower, tower.transform.position, tower.transform.rotation);
            tower.SetActive(true);
            troopArea.SetActive(true);
            troopArea.GetComponent<SphereCollider>().radius = towerData.atkRaio;

            t1 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(0).position, troopArea.transform.GetChild(0).rotation);
            t2 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(1).position, troopArea.transform.GetChild(1).rotation);
            t3 = Instantiate(towerData.gameModelTroop, troopArea.transform.GetChild(2).position, troopArea.transform.GetChild(2).rotation);

            // Set attack type here
            attack.SetAtk(towerData.atkSpeed + bonusAtkSpeed, towerData.atkDamage + bonusAtk, t1.transform.GetChild(0).GetComponent<Animator>(), t2.transform.GetChild(0).GetComponent<Animator>(), t3.transform.GetChild(0).GetComponent<Animator>(), towerData.attackType);
        }
    }


    public void SetBonusRelic(string nameTower)
    {

        bonusAtk = 0;
        bonusAtkSpeed = 0;
        bonusCustTower = 0;

        for (int iRelic = 0; iRelic < gameManager.reliquias.Count; iRelic++)
        {

            if (gameManager.reliquias[iRelic].tower != "")
            {

                if (nameTower == gameManager.reliquias[iRelic].tower)
                {

                    bonusAtk += gameManager.reliquias[iRelic].towerAtkDamage;
                    bonusAtkSpeed += gameManager.reliquias[iRelic].towerAtkSpeed;
                    bonusCustTower += gameManager.reliquias[iRelic].custTower;

                }

            }
            else
            {

                if (gameManager.GetTower(nameTower).tipoRecurso[0] == gameManager.reliquias[iRelic].tipoRecurso)
                {

                    bonusAtk += gameManager.reliquias[iRelic].towerAtkDamage;
                    bonusAtkSpeed += gameManager.reliquias[iRelic].towerAtkSpeed;
                    bonusCustTower += gameManager.reliquias[iRelic].custTower;

                }

            }

        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(viewAreaAtk)
        {

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(troopArea.transform.position, towerData.atkRaio);

        }         

    }
#endif
}
