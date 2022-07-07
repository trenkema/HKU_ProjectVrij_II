using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class CollisionOverlap : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger = new UnityEvent();
    [SerializeField] IntEvent intEvent = new IntEvent();
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, layerMask))
        {
            onTrigger?.Invoke();

            if (collision.transform.TryGetComponent(out PhotonView PV))
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
