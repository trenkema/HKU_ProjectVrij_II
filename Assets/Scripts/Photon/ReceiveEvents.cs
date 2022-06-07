using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ReceiveEvents : MonoBehaviour
{
    [Header("Event Codes")]
    [SerializeField] byte destroySpiderEventCode = 1;
    [SerializeField] byte respawnSpiderEventCode = 2;
    [SerializeField] byte gameWonEventCode = 3;
    [SerializeField] byte spiderDestroyedEventCode = 4;
    [SerializeField] byte updateScoreEventCode = 5;

    PhotonView PV;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(gameObject);
        }
    }

    private void OnEvent(EventData _photonEvent)
    {
        byte eventCode = _photonEvent.Code;

        if (eventCode == destroySpiderEventCode)
        {
            object[] data = (object[])_photonEvent.CustomData;

            // Check if the Event is send to me
            if (PhotonView.Find((int)data[0]).IsMine)
            {
                EventSystemNew.RaiseEvent(Event_Type.SPIDER_DESTROY_CAMERA);

                // Find PhotonView with ViewID Data
                PhotonNetwork.Destroy(PhotonView.Find((int)data[0]).gameObject);

                object[] content = new object[] { PV.ViewID };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

                PhotonNetwork.RaiseEvent(spiderDestroyedEventCode, content, raiseEventOptions, SendOptions.SendReliable);

                EventSystemNew<bool, bool>.RaiseEvent(Event_Type.SPIDER_DIED, true, (bool)data[1]);
            }
        }

        if (eventCode == spiderDestroyedEventCode)
        {
            EventSystemNew<bool, bool>.RaiseEvent(Event_Type.SPIDER_DIED, false, false);
        }

        if (eventCode == respawnSpiderEventCode)
        {
            object[] data = (object[])_photonEvent.CustomData;

            // Check if the Event is send to me
            if (PhotonView.Find((int)data[0]).IsMine)
            {
                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_RESPAWNED, true);
            }
            else
            {
                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_RESPAWNED, false);
            }
        }

        if (eventCode == gameWonEventCode)
        {
            object[] data = (object[])_photonEvent.CustomData;

            EventSystemNew<string>.RaiseEvent(Event_Type.GAME_WON, (string)data[0]);
        }

        if (eventCode == updateScoreEventCode)
        {
            object[] data = (object[])_photonEvent.CustomData;

            Player[] players = PhotonNetwork.PlayerList;

            Player correctPlayer = null;

            foreach (Player player in players)
            {
                if (player.ActorNumber == (int)data[0])
                {
                    correctPlayer = player;
                }
            }

            EventSystemNew<Player, int>.RaiseEvent(Event_Type.UPDATE_SCORE, correctPlayer, (int)data[1]);
        }
    }
}
