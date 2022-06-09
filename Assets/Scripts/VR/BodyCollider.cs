using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class BodyCollider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] XROrigin xrRig;

    [SerializeField] Transform cameraOffset;

    [SerializeField] CharacterController controller;

    [SerializeField] Transform target;

    private void Start()
    {
        StartCoroutine(SetOffset());
    }

    private IEnumerator SetOffset()
    {
        yield return new WaitForSeconds(0.5f);

        cameraOffset.localPosition = new Vector3(cameraOffset.localPosition.x, -0.25f, cameraOffset.localPosition.z);
    }

    private void Update()
    {
        if (xrRig.CameraInOriginSpacePos != null)
        {
            controller.center = new Vector3(target.localPosition.x, controller.height / 2 + controller.skinWidth, target.localPosition.z);

            controller.height = xrRig.CameraInOriginSpaceHeight;

            controller.Move(new Vector3(0.001f, -0.001f, 0.001f));
            controller.Move(new Vector3(-0.001f, -0.001f, -0.001f));
        }
    }
}
