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
            if (PV.ViewID == (int)data[0])
            {
                // Find PhotonView with ViewID Data
                PhotonNetwork.Destroy(PhotonView.Find((int)data[0]).gameObject);

                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_DIED, true);
            }
            else
            {
                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_DIED, false);
            }
        }

        if (eventCode == respawnSpiderEventCode)
        {
            object[] data = (object[])_photonEvent.CustomData;

            // Check if the Event is send to me
            if (PV.ViewID == (int)data[0])
            {
                // Find PhotonView with ViewID Data
                PhotonNetwork.Destroy(PhotonView.Find((int)data[0]).gameObject);

                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_RESPAWNED, true);
            }
            else
            {
                EventSystemNew<bool>.RaiseEvent(Event_Type.SPIDER_RESPAWNED, false);
            }
        }
    }
}
