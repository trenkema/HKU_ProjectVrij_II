using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Mouth : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    [SerializeField] int scoreToEarn = 1;

    [SerializeField] string spiderTag;

    [SerializeField] byte destroySpiderEventCode = 1;

    [SerializeField] byte updateScoreEventCode = 5;

    Hashtable playerScore = new Hashtable();

    bool canScore = true;

    private void OnEnable()
    {
        EventSystemNew<string>.Subscribe(Event_Type.GAME_WON, GameWon);
    }

    private void OnDisable()
    {
        EventSystemNew<string>.Unsubscribe(Event_Type.GAME_WON, GameWon);
    }

    private void GameWon(string _playerName)
    {
        canScore = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine)
            return;

        if (collision.transform.CompareTag(spiderTag) && canScore)
        {
            int viewID = collision.transform.GetComponent<PhotonView>().ViewID;

            Player player = PhotonView.Find(viewID).Owner;

            int currentScore = 0;

            if (player.CustomProperties.ContainsKey("Score"))
            {
                currentScore = (int)player.CustomProperties["Score"];
            }

            currentScore += scoreToEarn;

            playerScore["Score"] = currentScore;

            player.SetCustomProperties(playerScore);

            // Destroy Spider
            object[] contentDestroy = new object[] { viewID, false };

            RaiseEventOptions raiseEventOptionsDestroy = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(destroySpiderEventCode, contentDestroy, raiseEventOptionsDestroy, SendOptions.SendReliable);

            // Update Score
            //object[] contentScore = new object[] { playerID, scoreToEarn };

            //RaiseEventOptions raiseEventOptionsScore = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            //PhotonNetwork.RaiseEvent(updateScoreEventCode, contentScore, raiseEventOptionsScore, SendOptions.SendReliable);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(spiderTag) && canScore)
    //    {
    //        int viewID = other.GetComponent<PhotonView>().ViewID;
    //        int player = PhotonView.Find(viewID).Owner.ActorNumber;

    //        // Destroy Spider
    //        object[] contentDestroy = new object[] { viewID, false };

    //        RaiseEventOptions raiseEventOptionsDestroy = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    //        PhotonNetwork.RaiseEvent(destroySpiderEventCode, contentDestroy, raiseEventOptionsDestroy, SendOptions.SendReliable);

    //        // Update Score
    //        object[] contentScore = new object[] { player, scoreToEarn };

    //        RaiseEventOptions raiseEventOptionsScore = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    //        PhotonNetwork.RaiseEvent(updateScoreEventCode, contentScore, raiseEventOptionsScore, SendOptions.SendReliable);
    //    }
    //}
}
