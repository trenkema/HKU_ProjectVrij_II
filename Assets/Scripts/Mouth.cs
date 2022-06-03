using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mouth : MonoBehaviour
{
    [SerializeField] int lives = 1;

    [SerializeField] string spiderTag;

    [SerializeField] PhotonView PV;

    bool hasEnteredMouth = false;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.SPIDER_RESPAWNED, SpiderRespawned);
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.SPIDER_RESPAWNED, SpiderRespawned);
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void SpiderRespawned()
    {
        hasEnteredMouth = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spiderTag) && !hasEnteredMouth)
        {
            hasEnteredMouth = true;

            PV.RPC("RPC_MouthEntered", RpcTarget.Others);
        }
    }

    private void MouthEntered()
    {
        lives--;

        if (lives <= 0)
        {
            PV.RPC("RPC_GameOver", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_MouthEntered()
    {
        if (!PV.IsMine)
            return;

        Debug.Log("Mouth Entered");

        MouthEntered();
    }

    [PunRPC]
    public void RPC_GameOver()
    {
        Debug.Log("Game Over");
    }
}
