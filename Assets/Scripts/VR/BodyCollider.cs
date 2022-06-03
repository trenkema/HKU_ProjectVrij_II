using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using Photon.Pun;

public class BodyCollider : MonoBehaviourPun, IPunObservable
{
    [Header("References")]
    [SerializeField] PhotonView PV;

    [SerializeField] XROrigin xrRig;

    [SerializeField] CharacterController controller;

    [SerializeField] Transform target;

    private void Update()
    {
        if (!PV.IsMine)
            return;

        if (xrRig.CameraInOriginSpacePos != null)
        {
            controller.center = new Vector3(target.localPosition.x, controller.height / 2 + controller.skinWidth, target.localPosition.z);

            controller.height = xrRig.CameraInOriginSpaceHeight;

            controller.Move(new Vector3(0.001f, -0.001f, 0.001f));
            controller.Move(new Vector3(-0.001f, -0.001f, -0.001f));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(controller.center);
            stream.SendNext(controller.height);
        }
        else if (stream.IsReading)
        {
            controller.center = (Vector3)stream.ReceiveNext();
            controller.height = (float)stream.ReceiveNext();
        }
    }
}
