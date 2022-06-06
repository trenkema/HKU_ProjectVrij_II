using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] string spiderTag;

    [SerializeField] byte destroySpiderEventCode = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(spiderTag))
        {
            object[] content = new object[] { collision.transform.GetComponent<PhotonView>().ViewID };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
