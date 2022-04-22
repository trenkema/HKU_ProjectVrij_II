using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRStartSettings : MonoBehaviour
{
    [SerializeField] Transform playerTransform;

    [SerializeField] float vrStartDelay = 0.25f;

    [SerializeField] float cameraOffset = -0.25f;

    [SerializeField] Transform cameraOffsetTransform;

    [SerializeField] PlayerController playerController;

    private void Start()
    {
        StartCoroutine(SetupVR());
    }

    private IEnumerator SetupVR()
    {
        yield return new WaitForSeconds(vrStartDelay);

        playerTransform.localPosition = new Vector3(0f, 1f, 0f);

        playerController.useGravity = true;

        cameraOffsetTransform.transform.localPosition = new Vector3(cameraOffsetTransform.transform.position.x, cameraOffset, cameraOffsetTransform.transform.position.z);
    }
}
