using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRigidbody : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Rigidbody rb))
        {
            transform.SetParent(collision.transform);

            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Rigidbody rb))
        {
            transform.SetParent(null);

            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
