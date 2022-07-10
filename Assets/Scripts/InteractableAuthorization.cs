using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractableAuthorization : MonoBehaviour
{
    [SerializeField] PhotonView[] photonViewsToTakeOver;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (GameManager.Instance.isVR)
        {
            foreach (var photonView in photonViewsToTakeOver)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }
    }
}
