using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandActions : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ActionBasedController controller;

    [SerializeField] LayerMask grabbableLayer;

    [SerializeField] Transform palm;

    [SerializeField] float reachDistance = 0.049f, joinDistance = 0.05f;

    Transform followTarget;

    GameObject heldObject;
    Transform grabPoint;

    bool isGrabbing;

    private void Start()
    {
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;
    }

    private void Grab(InputAction.CallbackContext context)
    {
        if (isGrabbing || heldObject) return;

        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);

        Debug.Log("Amount: " + grabbableColliders.Length);

        if (grabbableColliders.Length < 1) return;

        var objectToGrab = grabbableColliders[0].transform.gameObject;
        var objectBody = objectToGrab.GetComponent<Rigidbody>();
        if (objectBody != null)
        {
            heldObject = objectBody.gameObject;
        }
        else
        {
            objectBody = objectToGrab.GetComponentInParent<Rigidbody>();
            if (objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                return;
            }
        }

        StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
    }

    private IEnumerator GrabObject(Collider collider, Rigidbody objectBody)
    {
        isGrabbing = true;
        // Create a grab point
        grabPoint = new GameObject().transform;
        grabPoint.position = collider.ClosestPoint(palm.position);
        grabPoint.parent = heldObject.transform;

        // Move hand to grab points
        followTarget = grabPoint;

        // Wait for hand to reach grab point
        while (grabPoint != null && Vector3.Distance(grabPoint.position, palm.position) > joinDistance && isGrabbing)
        {
            yield return new WaitForEndOfFrame();
        }

        // Freeze hand and object motion
        objectBody.velocity = Vector3.zero;
        objectBody.angularVelocity = Vector3.zero;

        objectBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        objectBody.interpolation = RigidbodyInterpolation.Interpolate;

        // BUGGY
        // Attach joints
        //joint1 = gameObject.AddComponent<FixedJoint>();
        //joint1.connectedBody = objectBody;
        //joint1.breakForce = float.PositiveInfinity;
        //joint1.breakTorque = float.PositiveInfinity;

        //joint1.connectedMassScale = 1;
        //joint1.massScale = 1;
        //joint1.enableCollision = false;
        //joint1.enablePreprocessing = false;

        //body.velocity = Vector3.zero;
        //body.angularVelocity = Vector3.zero;

        //joint2 = heldObject.AddComponent<FixedJoint>();
        //joint2.connectedBody = body;
        //joint2.breakForce = float.PositiveInfinity;
        //joint2.breakTorque = float.PositiveInfinity;

        //joint2.connectedMassScale = 1;
        //joint2.massScale = 1;
        //joint2.enableCollision = false;
        //joint2.enablePreprocessing = false;

        // Reset follow target
        followTarget = controller.gameObject.transform;
    }

    private void Release(InputAction.CallbackContext context)
    {
        // BUGGY
        //if (joint1 != null)
        //{
        //    Destroy(joint1);
        //    joint1 = null;
        //}
        //if (joint2 != null)
        //{
        //    Destroy(joint2);
        //    joint2 = null;
        //}
        if (grabPoint != null)
        {
            Destroy(grabPoint.gameObject);
            grabPoint = null;
        }

        if (heldObject != null)
        {
            var objectBody = heldObject.GetComponent<Rigidbody>();
            objectBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            objectBody.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
        }

        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }
}
