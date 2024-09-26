using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnExit;


    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit?.Invoke();
    }
}
