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

    [SerializeField] float destroyTime = 3;

    PhotonView PV;

    bool isDestroyed = false;

    void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            //SOUND
            object[] content = new object[] { (int)Sound_Type.WebTrail, PV.ViewID, true };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent((int)Event_Code.SoundTrigger, content, raiseEventOptions, SendOptions.SendReliable);

            StartCoroutine(DestroyTrail());
        }
    }

    IEnumerator DestroyTrail()
    {
        yield return new WaitForSeconds(destroyTime);

        if (!isDestroyed)
        {
            isDestroyed = true;

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void Setup(Collider PlayerCollider)
    {
        Physics.IgnoreCollision(PlayerCollider, GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (PV.IsMine)
            {
                if (!isDestroyed)
                {
                    isDestroyed = true;

                    PhotonNetwork.Instantiate(webPrefabName, transform.position, Quaternion.identity);

                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
