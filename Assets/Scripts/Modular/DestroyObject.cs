using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private PhotonView PV;

    bool isDestroyed = false;

    public void Destroy(GameObject _objectToDestroy)
    {
        if (!isDestroyed)
        {
            if (!PV.IsMine)
            {
                return;
            }

            isDestroyed = true;

            PhotonNetwork.Destroy(_objectToDestroy);
        }
    }

    public void Destroy(int _photonViewID)
    {
        if (!isDestroyed)
        {
            if (!PV.IsMine)
            {
                return;
            }

            isDestroyed = true;

            GameObject gObj = PhotonView.Find(_photonViewID).gameObject;
            PhotonNetwork.Destroy(gObj);
        }
    }

    public void OutOfBorder(int _photonViewID)
    {
        if (!PV.IsMine)
        {
            return;
        }

        object[] content = new object[] { _photonViewID, false };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
