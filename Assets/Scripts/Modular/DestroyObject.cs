using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private PhotonView PV;

    private bool isDestroyed = false;

    public void Destroy(GameObject _gObj)
    {
        if (!isDestroyed)
        {
            if (!PV.IsMine)
            {
                return;
            }

            isDestroyed = true;

            PhotonNetwork.Destroy(_gObj);
        }
    }

    public void DestroySpider(GameObject _gObj)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (_gObj.TryGetComponent(out PhotonView photonView))
        {
            object[] content = new object[] { photonView.ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void OutOfBorder(GameObject _gObj)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (_gObj.TryGetComponent(out PhotonView photonView))
        {
            object[] content = new object[] { photonView.ViewID, false };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
