using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Web : MonoBehaviour
{
    [SerializeField] int timeToDestroyWeb = 5;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            //SOUND
            object[] content = new object[] { (int)Sound_Type.WebImpact, PV.ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent((int)Event_Code.SoundTrigger, content, raiseEventOptions, SendOptions.SendReliable);

            StartCoroutine(WaitToDestroy());
        }
    }

    public void DestroyWeb()
    {

    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(timeToDestroyWeb);

        PhotonNetwork.Destroy(gameObject);
    }
}
