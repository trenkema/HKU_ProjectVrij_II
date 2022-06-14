using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVentilator : MonoBehaviour
{

    [SerializeField] float forwardForce = 1f;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField] GameObject blades;

    private void Update()
    {
        blades.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag != "Player")
        {
            other.attachedRigidbody.AddForce(transform.forward * forwardForce);
            //Debug.Log(other.attachedRigidbody);
        }
    }
}
