using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WebTrail : MonoBehaviour
{
    [SerializeField] string webPrefabName;

    [SerializeField] float destroyTime = 3;

    GameObject spawnedWeb;

    PhotonView PV;

    bool isDestroyed = false;

    void Awake()
    {
        PV = GetComponent<PhotonView>();

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

    void Start()
    {
        transform.Rotate(0, 90, 90);
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

                    spawnedWeb = PhotonNetwork.Instantiate(webPrefabName, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
