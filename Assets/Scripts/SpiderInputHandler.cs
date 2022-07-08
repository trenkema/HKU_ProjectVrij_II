using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderInputHandler : MonoBehaviour
{
    public void Move(InputAction.CallbackContext _context)
    {
        Debug.Log("Move");

        Vector2 input = _context.ReadValue<Vector2>();
        EventSystemNew<float, float>.RaiseEvent(Event_Type.Move, input.x, input.y);
    }

    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Jump");

            EventSystemNew.RaiseEvent(Event_Type.Jump);
        }
    }

    public void Shoot(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Shoot");

            EventSystemNew.RaiseEvent(Event_Type.Shoot);
        }
    }

    public void Swing(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Swing");

            EventSystemNew<bool>.RaiseEvent(Event_Type.Swing, true);
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            EventSystemNew<bool>.RaiseEvent(Event_Type.Swing, false);
        }
    }

    public void Fall(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Fall");

            EventSystemNew<bool>.RaiseEvent(Event_Type.Fall, true);
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            EventSystemNew<bool>.RaiseEvent(Event_Type.Fall, false);
        }
    }

    public void ChangeSpectator(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Change Spectator");

            float value = _context.ReadValue<Vector2>().x;

            EventSystemNew<int>.RaiseEvent(Event_Type.ChangeSpectator, (int)value);
        }
    }

    public void ForceRespawn(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Force Respawn");

            EventSystemNew.RaiseEvent(Event_Type.ForceRespawn);
        }
    }

    public void RopeForward(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            Debug.Log("Rope Forward");

            EventSystemNew<bool>.RaiseEvent(Event_Type.RopeForward, true);
        }
        else if (_context.phase == InputActionPhase.Canceled)
        {
            EventSystemNew<bool>.RaiseEvent(Event_Type.RopeForward, false);
        }
    }
}
