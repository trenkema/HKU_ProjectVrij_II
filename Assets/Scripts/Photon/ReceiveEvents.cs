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
        if (!PV.IsMine)
        {
            return;
        }

        byte eventCode = _photonEvent.Code;

        object[] data = (object[])_photonEvent.CustomData;

        if (eventCode == destroySpiderEventCode)
        {
            // Find PhotonView with ViewID Data
            PhotonNetwork.Destroy(PhotonView.Find((int)data[0]).gameObject);
        }
    }
}
