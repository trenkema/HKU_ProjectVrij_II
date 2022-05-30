using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SyncTransform : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        PV.RPC("RPC_SyncTransform", RpcTarget.Others, transform.position.x, transform.position.y, transform.position.z);

        Debug.Log("Pos: " + transform.position);
    }

    [PunRPC]
    public void RPC_SyncTransform(float _xPos, float _yPos, float _zPos)
    {
        transform.position = new Vector3(_xPos, _yPos, _zPos);

        Debug.Log("Pos Y: " + _yPos);
    }
}
