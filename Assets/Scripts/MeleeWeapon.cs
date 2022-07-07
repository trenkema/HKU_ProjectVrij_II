using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] string spiderTag;

    [SerializeField] PhotonView PV;

    bool canKill = true;

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
        if (!PV.IsMine)
        {
            Destroy(this);
        }
    }

    private void GameWon(string _playerName)
    {
        canKill = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(spiderTag) && canKill)
        {
            int viewID = collision.transform.GetComponent<PhotonView>().ViewID;

            object[] content = new object[] { viewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent((int)Event_Code.DestroySpider, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
