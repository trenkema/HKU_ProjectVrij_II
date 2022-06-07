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

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(spiderTag))
        {
            object[] content = new object[] { collision.transform.GetComponent<PhotonView>().ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
