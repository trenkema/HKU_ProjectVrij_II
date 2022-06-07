using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Mouth : MonoBehaviour
{
    [SerializeField] int scoreToEarn = 1;

    [SerializeField] string spiderTag;

    [SerializeField] byte destroySpiderEventCode = 1;

    [SerializeField] byte updateScoreEventCode = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spiderTag))
        {
            int viewID = other.GetComponent<PhotonView>().ViewID;

            // Destroy Spider
            object[] content = new object[] { viewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);

            // Update Score
            object[] content2 = new object[] { viewID, scoreToEarn };

            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content2, raiseEventOptions2, SendOptions.SendReliable);
        }
    }
}
