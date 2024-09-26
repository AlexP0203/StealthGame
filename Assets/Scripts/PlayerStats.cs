using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] Stat maxHealth;
    [SerializeField] Stat currentHealth;
    void Start()
    {
        currentHealth.amount = maxHealth.amount;
    }
}
