using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ActionBasedController controller;

    [SerializeField] Animator animator;

    [Header("Settings")]
    [SerializeField] float animationSpeed = 10f;

    [SerializeField] string animatorGripParam = "Grip";
    [SerializeField] string animatorTriggerParam = "Trigger";

    float gripTarget;
    float triggerTarget;
    float gripCurrent;
    float triggerCurrent;

    private void Update()
    {
        SetGrip(controller.selectAction.action.ReadValue<float>());
        SetTrigger(controller.activateAction.action.ReadValue<float>());

        AnimateHand();
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    void AnimateHand()
    {
        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }
        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }
    }
}
