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
            // Destroy Spider
            object[] content = new object[] { other.GetComponent<PhotonView>().ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);

            // Update Score
            object[] content2 = new object[] { other.GetComponent<PhotonView>().ViewID, scoreToEarn };

            RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content2, raiseEventOptions2, SendOptions.SendReliable);
        }
    }
}
