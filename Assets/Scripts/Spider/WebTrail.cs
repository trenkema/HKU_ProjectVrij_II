using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using FMOD.Studio;

public class WebTrail : MonoBehaviour
{
    [SerializeField] string webPrefabName;

    [SerializeField] PhotonView PV;

    public void CreateWeb()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Instantiate(webPrefabName, transform.position, Quaternion.identity);
        }
    }

    public void Setup(Collider PlayerCollider)
    {
        Physics.IgnoreCollision(PlayerCollider, GetComponent<Collider>());
    }
}
