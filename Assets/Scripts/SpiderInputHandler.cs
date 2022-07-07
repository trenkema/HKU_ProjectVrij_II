using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiderInputHandler : MonoBehaviour
{
    public void Move(InputAction.CallbackContext _context)
    {

    }

    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            EventSystemNew.RaiseEvent(Event_Type.Jump);
        }
    }

    public void Shoot(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            EventSystemNew.RaiseEvent(Event_Type.Shoot);
        }
    }

    public void Swing(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
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
            float value = _context.ReadValue<Vector2>().x;

            EventSystemNew<int>.RaiseEvent(Event_Type.ChangeSpectator, (int)value);
        }
    }

    public void Respawn(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {

        }
    }
}
