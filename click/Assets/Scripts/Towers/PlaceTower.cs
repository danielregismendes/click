using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

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
                        StopAllCoroutines(); // Stop any ongoing single-click coroutine
                        AttemptToActivateSlot(true);
                    }
                    else
                    {
                        // Start a coroutine to handle the single-click action after a short delay
                        StartCoroutine(HandleSingleClick());
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

    private IEnumerator HandleSingleClick()
    {
        // Wait for a short period to allow for a possible double-click
        yield return new WaitForSeconds(doubleClickThreshold);

        // If this point is reached, it means no double-click was detected, so handle the single-click action
        DisplayDormantMessage();
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }



    private void AttemptToActivateSlot(bool doubleClick)
    {
        if (doubleClick)
        {
            int currentMoeda = gameManager.GetInventario("Moeda");

            if (currentMoeda >= moedaCostToActivate)
            {
                // Deduct the required "moeda" to activate the slot
                gameManager.SetInventario("Moeda", -moedaCostToActivate);
                uiManager.AtualizarUI(); // Update the UI to reflect the new resource count

                // Transition the state to VAZIO
                state = TOWERSTATE.VAZIO;

                // Show the "Torre despertada" message
                DisplayAwakenedMessage();
            }
            else
            {
                // Show the insufficient "moeda" message
                DisplayInsufficientMoedaMessage();
            }
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
            
            float slowPercentage = towerData.slowPercentage;

            // Set attack type here
            attack.SetAtk(towerData.atkSpeed + bonusAtkSpeed,
                          towerData.atkDamage + bonusAtk,
                          t1.transform.GetChild(0).GetComponent<Animator>(),
                          t2.transform.GetChild(0).GetComponent<Animator>(),
                          t3.transform.GetChild(0).GetComponent<Animator>(),
                          towerData.attackType,
                          towerData.slowPercentage);  // Add the slowPercentage parameter here

        }

    }

    private void DisplayDormantMessage()
    {
        // Create a new GameObject to display the text
        GameObject dormantTextObject = new GameObject("DormantText");
        dormantTextObject.transform.position = transform.position + new Vector3(0, 15.0f, 0);  // Adjust Y position as needed

        // Set the layer to "UI"
        dormantTextObject.layer = LayerMask.NameToLayer("UI");

        // Add a TextMeshPro component and set up the properties
        TextMeshPro textMeshPro = dormantTextObject.AddComponent<TextMeshPro>();
        textMeshPro.text = $"Torre em estado Dormente, gaste {moedaCostToActivate} moedas para despertar.";
        textMeshPro.enableAutoSizing = false;
        textMeshPro.fontSize = 50;  // Set font size for visibility
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.yellow;  // Set color for the text
        textMeshPro.fontStyle = FontStyles.Bold;

        // Set the transform scale appropriately
        textMeshPro.transform.localScale = Vector3.one;

        // Adjust the bounding box size to give more room for the text
        RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(500, 200);  // Increase width to avoid line breaks

        // Set the sorting order extremely high to make sure the text is always rendered on top
        textMeshPro.sortingOrder = 1000;  // Set a high value to ensure it's rendered above other objects

        // Add a simple script to make the text always face the camera
        dormantTextObject.AddComponent<Billboard>();

        // Set the render mode to overlay
        textMeshPro.GetComponent<Renderer>().material.renderQueue = 4000; // Forces overlay rendering

        // Destroy the text after 2 seconds
        Destroy(dormantTextObject, 2.0f);
    }

    private void DisplayInsufficientMoedaMessage()
    {
        // Create a new GameObject to display the text
        GameObject insufficientMoedaTextObject = new GameObject("InsufficientMoedaText");
        insufficientMoedaTextObject.transform.position = transform.position + new Vector3(0, 15.0f, 0);  // Adjust Y position as needed

        // Set the layer to "UI"
        insufficientMoedaTextObject.layer = LayerMask.NameToLayer("UI");

        // Add a TextMeshPro component and set up the properties
        TextMeshPro textMeshPro = insufficientMoedaTextObject.AddComponent<TextMeshPro>();
        textMeshPro.text = "Moedas insuficientes para despertar a torre";
        textMeshPro.enableAutoSizing = false;
        textMeshPro.fontSize = 50;  // Set font size for visibility
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.red;  // Set color for the text
        textMeshPro.fontStyle = FontStyles.Bold;

        // Set the transform scale appropriately
        textMeshPro.transform.localScale = Vector3.one;

        // Adjust the bounding box size to give more room for the text
        RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(500, 200);  // Increase width to avoid line breaks

        // Set the sorting order extremely high to make sure the text is always rendered on top
        textMeshPro.sortingOrder = 1000;  // Set a high value to ensure it's rendered above other objects

        // Add a simple script to make the text always face the camera
        insufficientMoedaTextObject.AddComponent<Billboard>();

        // Set the render mode to overlay
        textMeshPro.GetComponent<Renderer>().material.renderQueue = 4000; // Forces overlay rendering

        // Destroy the text after 2 seconds
        Destroy(insufficientMoedaTextObject, 2.0f);
    }


    private void DisplayAwakenedMessage()
    {
        // Create a new GameObject to display the text
        GameObject awakenedTextObject = new GameObject("AwakenedText");
        awakenedTextObject.transform.position = transform.position + new Vector3(0, 15.0f, 0);  // Adjust Y position as needed

        // Set the layer to "UI"
        awakenedTextObject.layer = LayerMask.NameToLayer("UI");

        // Add a TextMeshPro component and set up the properties
        TextMeshPro textMeshPro = awakenedTextObject.AddComponent<TextMeshPro>();
        textMeshPro.text = "Torre despertada";
        textMeshPro.enableAutoSizing = false;
        textMeshPro.fontSize = 50;  // Set font size for visibility
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.color = Color.green;  // Set color for the text
        textMeshPro.fontStyle = FontStyles.Bold;

        // Set the transform scale appropriately
        textMeshPro.transform.localScale = Vector3.one;

        // Adjust the bounding box size to give more room for the text
        RectTransform rectTransform = textMeshPro.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(500, 200);  // Increase width to avoid line breaks

        // Set the sorting order extremely high to make sure the text is always rendered on top
        textMeshPro.sortingOrder = 1000;  // Set a high value to ensure it's rendered above other objects

        // Add a simple script to make the text always face the camera
        awakenedTextObject.AddComponent<Billboard>();

        // Set the render mode to overlay
        textMeshPro.GetComponent<Renderer>().material.renderQueue = 4000; // Forces overlay rendering

        // Destroy the text after 2 seconds
        Destroy(awakenedTextObject, 2.0f);
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
