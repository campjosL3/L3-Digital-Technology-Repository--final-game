using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmg : MonoBehaviour
{
    public float damage;
    public float damageInterval = 1f; // Time between damage ticks
    private float lastDamageTime;

    public CameraShake cameraShake;
    public GameObject cameraHolder; // Reference to the CameraHolder object

    private void Start()
    {
        lastDamageTime = Time.time;

        // Find the CameraShake script on the CameraHolder if not assigned
        if (cameraShake == null && cameraHolder != null)
        {
            cameraShake = cameraHolder.GetComponent<CameraShake>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Deal damage immediately when entering the trigger
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            lastDamageTime = Time.time; // Reset the timer
            Debug.Log("Initial Damage Dealt!");

            // Trigger the camera shake
            if (cameraShake != null)
            {
                cameraShake.TriggerShake(0.5f); // Adjust the shake duration as needed
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                other.GetComponent<PlayerStats>().TakeDamage(damage);
                lastDamageTime = Time.time;
                Debug.Log("Continuous Damage Dealt!");

                // Trigger the camera shake
                if (cameraShake != null)
                {
                    cameraShake.TriggerShake(0.5f); // Adjust the shake duration as needed
                }
            }
        }
    }
}
