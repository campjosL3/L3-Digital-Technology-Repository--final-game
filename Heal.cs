using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public float healAmount;
    public float healRate = 1f;
    private float lastHealTime;

    private void Start()
    {
        lastHealTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().SetInHealingArea(true);
            HealPlayer(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastHealTime + healRate)
            {
                HealPlayer(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().SetInHealingArea(false);
        }
    }

    private void HealPlayer(Collider player)
    {
        player.GetComponent<PlayerStats>().Heal(healAmount);
        lastHealTime = Time.time;
    }

    private void OnDestroy()
    {
        // Ensure that if the player is inside the healing area when this object is destroyed, the healing state is properly reset
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7f); // Adjust radius as needed
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerStats>().SetInHealingArea(false);
            }
        }
    }

}
