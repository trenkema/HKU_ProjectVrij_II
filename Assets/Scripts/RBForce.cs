using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RBForce : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                rb.AddForce(new Vector3(0, 0, 50));
            }
        }
    }
}
