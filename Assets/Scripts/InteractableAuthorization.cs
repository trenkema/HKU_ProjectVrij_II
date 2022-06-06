using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InteractableAuthorization : MonoBehaviour
{
    [SerializeField] PhotonView[] photonViewsToTakeOver;

    private void Start()
    {
        if (GameManager.Instance.isVR)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                foreach (var photonView in photonViewsToTakeOver)
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                }
            }
        }
    }
}
