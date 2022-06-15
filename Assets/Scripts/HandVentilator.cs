using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVentilator : MonoBehaviour
{
    [SerializeField] LayerMask layersToInteractWith;

    [SerializeField] float forwardForce = 1f;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField] GameObject blades;

    private void Update()
    {
        blades.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, layersToInteractWith))
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(transform.forward * forwardForce);
            }
        }
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
