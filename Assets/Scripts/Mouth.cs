using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Mouth : MonoBehaviour
{
    [SerializeField] int lives = 1;

    [SerializeField] string spiderTag;

    [SerializeField] byte destroySpiderEventCode = 1;

    [SerializeField] byte gameWonEventCode = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spiderTag))
        {
            object[] content = new object[] { other.GetComponent<PhotonView>().ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);

            MouthEntered();
        }
    }

    private void MouthEntered()
    {
        lives--;

        if (lives <= 0)
        {
            object[] content = new object[] { (int)PlayerTypes.Spiders };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent(gameWonEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
