using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SpiderCollisionCheck : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    [SerializeField] int scoreToEarn = 1;

    [SerializeField] string spiderEarnPointsTag;

    bool canScore = true;

    private void OnEnable()
    {
        EventSystemNew<string>.Subscribe(Event_Type.GAME_WON, GameWon);
    }

    private void OnDisable()
    {
        EventSystemNew<string>.Unsubscribe(Event_Type.GAME_WON, GameWon);
    }

    private void Start()
    {
        if (GameManager.Instance.gameEnded)
        {
            canScore = false;
        }
    }

    private void GameWon(string _playerName)
    {
        canScore = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine)
            return;

        if (collision.transform.CompareTag(spiderEarnPointsTag) && canScore)
        {
            if (collision.gameObject.TryGetComponent(out PointsToEarn pointsScript))
            {
                canScore = false;

                EventSystemNew.RaiseEvent(Event_Type.SPIDER_DESTROY_CAMERA);

                PhotonNetwork.Destroy(gameObject);

                object[] content = new object[] { PV.ViewID };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

                PhotonNetwork.RaiseEvent((int)Event_Code.SpiderDestroyed, content, raiseEventOptions, SendOptions.SendReliable);

                EventSystemNew<bool, bool>.RaiseEvent(Event_Type.SPIDER_DIED, true, false);

                // Increase Own Score
                Player player = PhotonNetwork.LocalPlayer;

                int currentScore = 0;

                if (player.CustomProperties.ContainsKey("Score"))
                {
                    currentScore = (int)player.CustomProperties["Score"];
                }

                currentScore += pointsScript.pointsToEarn;

                var hash = player.CustomProperties;

                hash["Score"] = currentScore;

                player.SetCustomProperties(hash);
            }
        }
    }
}
