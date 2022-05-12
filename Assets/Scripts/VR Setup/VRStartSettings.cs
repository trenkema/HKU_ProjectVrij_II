using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRStartSettings : MonoBehaviour
{
    [SerializeField] bool enableStartPosition = true;

    [SerializeField] Vector3 startPosition;

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

        if (enableStartPosition)
            playerTransform.localPosition = startPosition;

        playerController.useGravity = true;

        cameraOffsetTransform.transform.localPosition = new Vector3(cameraOffsetTransform.transform.localPosition.x, cameraOffset, cameraOffsetTransform.localPosition.z);
    }
}
