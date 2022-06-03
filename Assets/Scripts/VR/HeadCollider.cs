using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField] Transform cam;

    [SerializeField] CharacterController controller;

    private void Update()
    {
        controller.center = cam.localPosition;
    }
}
