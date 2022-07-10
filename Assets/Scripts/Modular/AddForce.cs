using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class AddForce : MonoBehaviour
{
    [SerializeField] float force = 1f;

    [SerializeField] private PhotonView PV;

    public void AddForwardForceToRB(GameObject _gObj)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (_gObj.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(transform.forward * force);
        }
    }
}
