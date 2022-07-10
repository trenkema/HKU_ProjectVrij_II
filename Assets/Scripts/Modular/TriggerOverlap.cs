using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TriggerOverlap : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger = new UnityEvent();
    [SerializeField] private UnityEvent onTriggerExit = new UnityEvent();
    [SerializeField] private UnityEvent<GameObject> gObjEvent = new UnityEvent<GameObject>();
    [SerializeField] private UnityEvent<GameObject> gObjEventExit = new UnityEvent<GameObject>();
    [SerializeField] private LayerMask layerMask = new LayerMask();

    [SerializeField] PhotonView PV;

    private void OnTriggerEnter(Collider other)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (IsInLayerMask(other.gameObject, layerMask))
        {
            onTrigger?.Invoke();

            gObjEvent?.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (IsInLayerMask(other.gameObject, layerMask))
        {
            onTriggerExit?.Invoke();

            gObjEventExit?.Invoke(other.gameObject);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
