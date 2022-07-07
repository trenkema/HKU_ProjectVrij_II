using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkDestroyObject : MonoBehaviour
{
    [SerializeField] private PhotonView PV;

    [SerializeField] private bool checkPVOwner = false;

    bool isDestroyed = false;

    public void NetworkDestroy(GameObject _objectToDestroy)
    {
        if (!isDestroyed)
        {
            if (checkPVOwner && !PV.IsMine)
            {
                return;
            }

            isDestroyed = true;

            PhotonNetwork.Destroy(_objectToDestroy);
        }
    }

    public void NetworkDestroy(int _photonViewID)
    {
        if (!isDestroyed)
        {
            if (checkPVOwner && !PV.IsMine)
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
        object[] content = new object[] { _photonViewID, false };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
