using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrain : MonoBehaviour
{
    [SerializeField] Stat maxHealth;
    [SerializeField] Stat currentHealth;

    bool setTrap;

    private void OnTriggerEnter(Collider other)
    {
        setTrap = true;
        StartCoroutine(DrainHealth());
    }

    private void OnTriggerExit(Collider other)
    {
        setTrap = false;
    }

    public void UserHealth(float amount)
    {
        currentHealth.amount -= (int)amount;

        if (currentHealth.amount <= 0)
        {
            currentHealth.amount = 0;
        }
    }
    IEnumerator DrainHealth()
    {
        while (setTrap)
        {
            currentHealth.amount -= 5;
            yield return new WaitForSeconds(2.0f);
        }
        yield return null;
    }
}