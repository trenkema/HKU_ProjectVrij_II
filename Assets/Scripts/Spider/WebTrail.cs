using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WebTrail : MonoBehaviour
{
    [SerializeField] string webPrefabName;

    [SerializeField] float destroyTime = 3;

    //SOUND

    private FMOD.Studio.EventInstance spiderTrailSound;
    private FMOD.Studio.EventInstance spiderTrailSoundEffect;

    GameObject spawnedWeb;

    PhotonView PV;

    bool isDestroyed = false;

    void Awake()
    {
        PV = GetComponent<PhotonView>();

        //SOUND
        spiderTrailSoundEffect = FMODUnity.RuntimeManager.CreateInstance("event:/WebEffect");
        spiderTrailSoundEffect.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        spiderTrailSoundEffect.start();

        if (PV.IsMine)
        {
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

                    PhotonNetwork.Destroy(gameObject);

                    //SOUND

                    spiderTrailSoundEffect.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                    spiderTrailSound = FMODUnity.RuntimeManager.CreateInstance("event:/WebCollider");
                    spiderTrailSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                    spiderTrailSound.start();

                    spawnedWeb = PhotonNetwork.Instantiate(webPrefabName, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
