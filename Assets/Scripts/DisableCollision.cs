using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableCollision : MonoBehaviour
{
    [SerializeField] private Collider[] collidersToDisable;

    [SerializeField] private Collider handColliderLeft;
    [SerializeField] private Collider handColliderRight;

    [SerializeField] private Collider[] socketCollider;
    [SerializeField] private Collider socketItemCollider;

    private void Start()
    {
        if (handColliderLeft == null || handColliderRight == null)
        {
            handColliderLeft = GameManager.Instance.leftHandPalmCollider;
            handColliderRight = GameManager.Instance.rightHandPalmCollider;
        }
    }

    public void Grabbed(SelectEnterEventArgs args)
    {
        if (args.interactor is XRDirectInteractor)
        {
            foreach (Collider collider in collidersToDisable)
            {
                Physics.IgnoreCollision(handColliderLeft, collider, true);
                Physics.IgnoreCollision(handColliderRight, collider, true);
            }
        }
    }

    public void Released(SelectExitEventArgs args)
    {
        if (args.interactor is XRDirectInteractor)
        {
            foreach (Collider collider in collidersToDisable)
            {
                Physics.IgnoreCollision(handColliderLeft, collider, false);
                Physics.IgnoreCollision(handColliderRight, collider, false);
            }
        }
    }

    public void DisableSocketCollision()
    {
        foreach (Collider collider in socketCollider)
        {
            Physics.IgnoreCollision(socketItemCollider, collider, true);
        }
    }

    public void EnableSocketCollision()
    {
        foreach (Collider collider in socketCollider)
        {
            Physics.IgnoreCollision(socketItemCollider, collider, false);
        }
    }
}
