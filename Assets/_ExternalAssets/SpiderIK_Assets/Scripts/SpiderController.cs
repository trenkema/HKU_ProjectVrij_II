/* 
 * This file is part of Unity-Procedural-IK-Wall-Walking-Spider on github.com/PhilS94
 * Copyright (C) 2020 Philipp Schofield - All Rights Reserved
 */

using UnityEngine;
using System.Collections;
using Raycasting;
using UnityEngine.InputSystem;

/*
 * This class needs a reference to the Spider class and calls the walk and turn functions depending on player input.
 * So in essence, this class translates player input to spider movement. The input direction is relative to a camera and so a 
 * reference to one is needed.
 */

[DefaultExecutionOrder(-1)] // Make sure the players input movement is applied before the spider itself will do a ground check and possibly add gravity
public class SpiderController : MonoBehaviour {

    [Header("Settings")]

    [SerializeField] float isFallingCheckTime = 0.25f;

    public Spider spider;

    [Header("Camera")]
    public SmoothCamera smoothCam;

    bool isFalling = false;

    bool canMove = false;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.GAME_STARTED, GameStarted);

        EventSystemNew.Subscribe(Event_Type.GAME_ENDED, GameEnded);

        if (GameManager.Instance.gameStarted && !GameManager.Instance.gameEnded)
        {
            canMove = true;

        }
        else if (GameManager.Instance.gameEnded)
        {
            canMove = false;

            smoothCam.XSensitivity = 0;
            smoothCam.YSensitivity = 0;
        }
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.GAME_STARTED, GameStarted);

        EventSystemNew.Unsubscribe(Event_Type.GAME_ENDED, GameEnded);
    }

    void FixedUpdate() {
        //** Movement **//
        Vector3 input = getInput();

        spider.walk(input);

        Quaternion tempCamTargetRotation = smoothCam.getCamTargetRotation();
        Vector3 tempCamTargetPosition = smoothCam.getCamTargetPosition();
        spider.turn(input);
        smoothCam.setTargetRotation(tempCamTargetRotation);
        smoothCam.setTargetPosition(tempCamTargetPosition);

        if (isFalling)
        {
            if (spider.GroundCheckFalling())
            {
                isFalling = false;

                spider.setGroundcheck(true);

                CancelInvoke();
            }
        }
    }

    private Vector3 getInput() {
        if (canMove)
        {
            Vector3 up = spider.transform.up;
            Vector3 right = spider.transform.right;
            Vector3 input = Vector3.ProjectOnPlane(smoothCam.getCameraTarget().forward, up).normalized * Input.GetAxis("Vertical") + (Vector3.ProjectOnPlane(smoothCam.getCameraTarget().right, up).normalized * Input.GetAxis("Horizontal"));
            Quaternion fromTo = Quaternion.AngleAxis(Vector3.SignedAngle(up, spider.getGroundNormal(), right), right);
            input = fromTo * input;
            float magnitude = input.magnitude;
            return (magnitude <= 1) ? input : input /= magnitude;
        }

        return Vector3.zero;
    }

    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            spider.Jump();
        }
    }

    public void Fall(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            if (spider.IsGrounded())
            {
                Invoke("SetFalling", isFallingCheckTime);

                spider.setGroundcheck(false);
            }
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            isFalling = false;

            spider.setGroundcheck(true);

            CancelInvoke();
        }
    }

    private void SetFalling()
    {
        isFalling = true;
    }

    private void GameStarted()
    {
        canMove = true;
    }

    private void GameEnded()
    {
        canMove = false;
    }
}