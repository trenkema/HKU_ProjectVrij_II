using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TriggerSound : MonoBehaviour
{
    [SerializeField] private Sound_Type soundType;

    [SerializeField] private PhotonView PV;

    public void Trigger()
    {
        if (!PV.IsMine)
        {
            return;
        }

        //SOUND
        object[] content = new object[] { (int)soundType, PV.ViewID, true };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent((int)Event_Code.SoundTrigger, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
