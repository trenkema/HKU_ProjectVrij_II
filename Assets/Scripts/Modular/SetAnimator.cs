using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void SetTrigger(string _triggerName)
    {
        animator.SetTrigger(_triggerName);
    }
}
