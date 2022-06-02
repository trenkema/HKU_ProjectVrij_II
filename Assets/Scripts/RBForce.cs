using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RBForce : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] float forwardForce = 50f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            rb.AddForce(new Vector3(0, 0, forwardForce));
        }
    }
}
