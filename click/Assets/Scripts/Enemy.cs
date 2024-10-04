using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace


public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Make sure the object faces the camera
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}


public class Enemy : MonoBehaviour {

    public Transform[] rota;
	public int rotaCount = 0;
	public int maxHealth;
    public int currentHealth;
	public int danoZigurate;

    [Header("Drop de Item")]
	public string itemDrop;
	public int qtdDrop;

    public float originalSpeed;
    public bool isSlowed = false;
    private NavMeshAgent navMesh;
	protected Animator anim;
	public bool isDead = false;
	private GameManager	gameManager;
	private UIManager uiManager;
	private int bonusDrop = 0;
    private EventInstance enemyFootsteps;

    public GameObject damageTextPrefab;  // Reference to a prefab with TextMesh or TextMeshPro
    private Transform cameraTransform;


    void Start () 
	{
        navMesh = GetComponent<NavMeshAgent>();
        originalSpeed = navMesh.speed;  // Store the initial speed
        anim = GetComponent<Animator>();
		currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>();
        navMesh.SetDestination(rota[rotaCount].position);
        uiManager = FindFirstObjectByType<UIManager>();
		SetBonusRelic();
        enemyFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.inimigo_steps);
        enemyFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        enemyFootsteps.start();

        // Reference the camera transform
        cameraTransform = Camera.main.transform;  // Assuming Camera.main is the active game camera
    }

    private void Update()
    {
        
        if(gameManager.stage == STAGEFASE.GAMEOVER) DisableEnemy();

    }

    void FixedUpdate()
	{
	
        AiRota();
        UpdateSound();

    }

    void AiRota()
    {
        if (isDead) return;  // Immediately return if the enemy is dead to prevent further actions

        if (navMesh.remainingDistance < navMesh.stoppingDistance + 1)
        {
            if (rotaCount >= rota.Length)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.litch_dano, gameObject.transform.position);
                gameManager.SetHpZigurate(danoZigurate);  // Apply damage to zigurate
                uiManager.AtualizarUI();  // Update UI
                enemyFootsteps.setPaused(true);  // Pause enemy footsteps
                DisableEnemy();  // Disable enemy when route ends
            }
            else
            {
                navMesh.SetDestination(rota[rotaCount].position);  // Move to the next waypoint
                rotaCount++;  // Increase waypoint count
            }
        }
    }

    public void TookDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");

            // Only display damage text if the damage is greater than 0
            if (damage > 0)
            {
                // Ensure there is a collider component to get the height
                Collider collider = GetComponent<Collider>();
                if (collider != null)
                {
                    // Create a new GameObject to display damage text
                    GameObject damageTextObject = new GameObject("DamageText");

                    // Increase the Y offset to make the text appear higher above the enemy
                    damageTextObject.transform.position = transform.position + new Vector3(0, collider.bounds.size.y + 9.0f, 0);

                    // Add a TextMeshPro component and set up the properties
                    TextMeshPro textMeshPro = damageTextObject.AddComponent<TextMeshPro>();
                    textMeshPro.text = damage.ToString();
                    textMeshPro.enableAutoSizing = false;  // Disable auto-sizing for better control
                    textMeshPro.fontSize = 50;  // Set font size for visibility
                    textMeshPro.alignment = TextAlignmentOptions.Center;
                    textMeshPro.color = Color.red;  // Set color for damage text
                    textMeshPro.fontStyle = FontStyles.Bold;

                    // Set the transform scale appropriately
                    textMeshPro.transform.localScale = Vector3.one;  // Set to a standard size

                    // Adjust text mesh properties for world space
                    textMeshPro.sortingOrder = 10;  // Set sorting order to render above other objects

                    // Add a simple script to make the text always face the camera
                    damageTextObject.AddComponent<Billboard>();

                    // Optional: Make the text move upwards and fade out over time
                    StartCoroutine(FadeOutAndDestroy(damageTextObject));
                }
            }

            if (currentHealth <= 0)
            {
                enemyFootsteps.stop(STOP_MODE.IMMEDIATE);
                enemyFootsteps.release();
                isDead = true;
                navMesh.enabled = false;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_morte, gameObject.transform.position);
                gameManager.SetInventario(itemDrop, qtdDrop + bonusDrop);
                uiManager.AtualizarUI();
                anim.SetBool("Death", true);
            }
        }
    }



    // Coroutine to make the damage text fade out and move upwards over time
    private IEnumerator FadeOutAndDestroy(GameObject damageTextObject)
    {
        TextMeshPro textMeshPro = damageTextObject.GetComponent<TextMeshPro>();
        float duration = 1.0f;  // Duration of fade out
        float elapsedTime = 0;

        Vector3 initialPosition = damageTextObject.transform.position;
        Vector3 finalPosition = initialPosition + new Vector3(0, 1, 0);  // Move up 1 unit

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Move upwards over time
            damageTextObject.transform.position = Vector3.Lerp(initialPosition, finalPosition, t);

            // Fade out over time
            textMeshPro.alpha = Mathf.Lerp(1, 0, t);

            yield return null;
        }

        Destroy(damageTextObject);
    }


    public void ApplySlow(float slowPercentage)
    {
        if (!isSlowed && navMesh != null && navMesh.isOnNavMesh && navMesh.enabled)
        {
            originalSpeed = navMesh.speed; // Store the original speed
            navMesh.speed = originalSpeed * (1 - slowPercentage / 100f); // Set the reduced speed directly

            // Only call isStopped if the agent is on a valid NavMesh and active
            if (navMesh.isOnNavMesh && navMesh.enabled)
            {
                navMesh.isStopped = true;
                navMesh.isStopped = false;
            }

            Debug.Log($"Applying slow: Original Speed = {originalSpeed}, New Speed = {navMesh.speed}"); // Debug line
            isSlowed = true;
        }
    }




    public void RemoveSlow()
    {
        if (isSlowed)
        {
            navMesh.speed = originalSpeed; // Restore original speed
            Debug.Log($"Removing slow: Restored Speed = {originalSpeed}"); // Debug line
            isSlowed = false;
        }
    }




    public void DisableEnemy()
	{

        enemyFootsteps.stop(STOP_MODE.IMMEDIATE);
        enemyFootsteps.release();

        if (isDead)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.inimigo_morte_contatoaosolo, Camera.main.transform.position);

        }

        Destroy(gameObject);

	}

	public int GetHealth()
	{

		return currentHealth;

	}

	public bool GetIsDead()
	{
		return isDead;

    }

	public void SetRota(Transform[] enemyRota)
	{

		rota = enemyRota;

	}

    public void SetBonusRelic()
    {

        for (int iRelic = 0; iRelic < gameManager.reliquias.Count; iRelic++)
        {

            if (gameManager.reliquias[iRelic].tower != "")
            {


            }
            else
            {

                if (itemDrop == gameManager.reliquias[iRelic].tipoRecurso)
                {

                    bonusDrop += gameManager.reliquias[iRelic].drop;

                }

            }

        }

    }

    private void UpdateSound()
    {
    
        enemyFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

    }

}
