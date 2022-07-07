using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Instantiated : MonoBehaviour
{
    [SerializeField] private UnityEvent onInstantiated = new UnityEvent();

    private void Awake()
    {
        onInstantiated?.Invoke();
    }
}
