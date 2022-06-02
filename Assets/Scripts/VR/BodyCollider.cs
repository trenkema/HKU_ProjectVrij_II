using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class BodyCollider : MonoBehaviour
{
    [SerializeField] XROrigin xrRig;

    [SerializeField] CharacterController controller;

    [SerializeField] Transform target;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] float gravity = -9.81f;

    float fallingSpeed;

    private void FixedUpdate()
    {
        if (xrRig.CameraInOriginSpacePos != null)
        {
            //var center = xrRig.CameraInOriginSpacePos;

            controller.center = new Vector3(target.position.x, controller.center.y, target.position.z);

            controller.height = xrRig.CameraInOriginSpaceHeight;

            //bool isGrounded = CheckIfGrounded();

            //if (isGrounded)
            //    fallingSpeed = 0;
            //else
            //{
            //    fallingSpeed += gravity * Time.fixedDeltaTime;
            //}

            //controller.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        }
    }

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(controller.center);
        float rayLength = controller.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, controller.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
