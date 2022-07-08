using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Instantiated : MonoBehaviour
{
    [SerializeField] private UnityEvent onInstantiated = new UnityEvent();

    [SerializeField] private PhotonView PV;

    private void Awake()
    {
        if (!PV.IsMine)
        {
            return;
        }

        onInstantiated?.Invoke();
    }
}
