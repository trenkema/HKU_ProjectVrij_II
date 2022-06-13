using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVentilator : MonoBehaviour
{

    [SerializeField] float forwardForce = 1f;

    [SerializeField] public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        rb.AddForce(transform.forward * forwardForce);
    }
}
