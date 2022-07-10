using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class CollisionOverlap : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger = new UnityEvent();
    [SerializeField] private UnityEvent onTriggerExit = new UnityEvent();
    [SerializeField] private UnityEvent<GameObject> gObjEvent = new UnityEvent<GameObject>();
    [SerializeField] private UnityEvent<GameObject> gObjEventExit = new UnityEvent<GameObject>();
    [SerializeField] private LayerMask layerMask = new LayerMask();

    [SerializeField] private PhotonView PV;

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (IsInLayerMask(collision.gameObject, layerMask))
        {
            onTrigger?.Invoke();

            gObjEvent?.Invoke(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (IsInLayerMask(collision.gameObject, layerMask))
        {
            onTriggerExit?.Invoke();

            gObjEventExit?.Invoke(collision.gameObject);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
