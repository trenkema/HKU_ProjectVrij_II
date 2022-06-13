using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Web : MonoBehaviour
{
    [SerializeField] int timeToDestroyWeb = 5;

    [SerializeField] byte soundEventCode = 8;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            //SOUND
            object[] content = new object[] { (int)Sound_Type.WebImpact, PV.ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(soundEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(timeToDestroyWeb);

        Destroy(this.gameObject);
    }
}
