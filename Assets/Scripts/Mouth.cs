using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Mouth : MonoBehaviour
{
    [SerializeField] int lives = 1;

    [SerializeField] string spiderTag;

    [SerializeField] PhotonView PV;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(spiderTag) && PV.IsMine)
        {
            PhotonNetwork.Destroy(other.gameObject);

            MouthEntered();
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
    public void RPC_GameOver()
    {
        Debug.Log("Game Over");

        EventSystemNew<PlayerTypes>.RaiseEvent(Event_Type.GAME_WON, PlayerTypes.Spiders);
    }
}
