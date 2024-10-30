using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 100f;

    public GameObject onDeathHeal;
    public float healLifeTime = 1.0f;

    public GameObject healEffect;
    public float effectLifeTime = 5.0f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Death();
    }

    private void Death()
    {
        if (onDeathHeal != null)
        {
            // Instantiate a new copy of the onDeathHeal object at the current position and rotation
            GameObject instantiatedHeal = Instantiate(onDeathHeal, transform.position, transform.rotation);
            // Instantiate heal explosion effect
            GameObject instantiatedEffect = Instantiate(healEffect, transform.position, transform.rotation);

            // Destroy the instantiated object after healLifeTime seconds
            Destroy(instantiatedHeal, healLifeTime);
            Destroy(instantiatedEffect, effectLifeTime);
        }

        // Destroy the Target object itself
        Destroy(gameObject);
        Debug.Log("Enemy Killed!");
    }
}
