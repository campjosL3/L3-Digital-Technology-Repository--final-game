using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    private float currentHealth;

    public HealthBar healthBar;

    public float lossRate;
    public float lossDmg;
    public float slideDmg = 0.6f;

    public GameObject criticalHealthScreen;
    public GameObject deathScreen;
    public GameObject Warning;

    private bool isInHealingArea = false;

    public float deathSlowDuration = 2f;
    public float deathSlowRate = 0.1f;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);

        criticalHealthScreen.SetActive(false);
        deathScreen.SetActive(false);
        Warning.SetActive(false);

        StartCoroutine(HealthDrain());

        playerMovement = GetComponent<PlayerMovement>();

        lossDmg = 0.1f;

    }

    public void TakeDamage(float amount) 
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
        CheckHealth();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        healthBar.SetSlider(currentHealth);
        Debug.Log("Player Healed!");

        if (currentHealth > maxHealth) 
        {
            currentHealth=maxHealth;
        }

        if (currentHealth >= 15)
        {
            criticalHealthScreen.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }

        if (playerMovement != null)
        {
            // Access the IsSliding property
            bool sliding = playerMovement.isSliding;

            // Do something with the sliding value
            if (sliding)
            {
                lossDmg = slideDmg;
                
                Warning.SetActive(true);
            }
            else
            {
                lossDmg = 0.1f;
                
                Warning.SetActive(false);
            }
        }

    }

    private void CheckHealth()
    {
        if (currentHealth <= 15)
        {
            criticalHealthScreen.SetActive(true);
            Debug.Log("Critical Health!");
        }


        if (currentHealth <= -1)
        {
            
            Debug.Log("You Died!");
            StartCoroutine(DeathSlow());
            criticalHealthScreen.SetActive(false);
            deathScreen.SetActive(true);
        }


    }

    private IEnumerator DeathSlow()
    {
        float startTimeScale = Time.timeScale;
        float elapsedTime = 0f;

        while (elapsedTime < deathSlowDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(startTimeScale, deathSlowRate, elapsedTime / deathSlowDuration);
            yield return null;
        }

        Time.timeScale = 0f;
        
    }

    private IEnumerator HealthDrain()
    {
        while (true)
        {
            yield return new WaitForSeconds(lossRate);
            if (!isInHealingArea)
            {
                TakeDamage(lossDmg);
            }
                
        }
    }

    public void SetInHealingArea(bool inHealingArea)
    {
        isInHealingArea = inHealingArea;
    }

}
