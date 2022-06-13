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

            PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, contentDestroy, raiseEventOptionsDestroy, SendOptions.SendReliable);
        }
    }
}
