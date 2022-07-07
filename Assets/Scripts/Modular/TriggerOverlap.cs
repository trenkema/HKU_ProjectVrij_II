using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TriggerOverlap : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger = new UnityEvent();
    [SerializeField] IntEvent intEvent = new IntEvent();
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, layerMask))
        {
            onTrigger?.Invoke();

            if (other.TryGetComponent(out PhotonView PV))
            {
                intEvent?.Invoke(PV.ViewID);
            }
        }
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
