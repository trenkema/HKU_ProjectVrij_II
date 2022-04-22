using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public LayerMask groundLayer;

    [SerializeField] private InputActionReference jumpActionReference;

    [SerializeField] private float jumpForce = 500.0f;
    [SerializeField] private bool jumpEnabled = true;
    [SerializeField] private float gravity = -9.81f;

    private XROrigin xrOrigin;
    private CharacterController character;
    private float fallingSpeed = 0;

    public bool useGravity = false;

    private bool isGrounded => Physics.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), Vector3.down, 2.0f);

    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
        character = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
        if (jumpEnabled)
        {
            jumpActionReference.action.performed += OnJump;
        }
    }

    void Update()
    {
        //capsuleColl.center = new Vector3(center.x, capsuleColl.center.y, center.z);
        //capsuleColl.height = xrRig.cameraInRigSpaceHeight;
    }

    private void FixedUpdate()
    {
        var center = xrOrigin.CameraInOriginSpacePos;
        character.center = new Vector3(center.x, character.center.y, center.z);
        character.height = xrOrigin.CameraInOriginSpaceHeight;

        bool isGrounded = CheckIfGrounded();

        if (isGrounded || !useGravity)
            fallingSpeed = 0;
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;
        //rb.AddForce(Vector3.up * jumpForce);
    }

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
