using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVentilator : MonoBehaviour
{

    [SerializeField] float forwardForce = 1f;

    void OnTriggerEnter(Collider other)
    {
        other.attachedRigidbody.AddForce(transform.forward * forwardForce);
        //Debug.Log(other.attachedRigidbody);
    }
}
