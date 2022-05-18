using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJointScript : MonoBehaviour
{
    public GameObject RopeBottom;

    [SerializeField] float force = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        RopeBottom.GetComponent<Rigidbody>().AddForce(transform.forward * Vertical * force, ForceMode.Acceleration);
        RopeBottom.GetComponent<Rigidbody>().AddForce(transform.right * Horizontal * force, ForceMode.Acceleration);
    }
}
