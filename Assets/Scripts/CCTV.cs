using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    [SerializeField] Camera[] cameras;

    [SerializeField] float updateInterval = 0.25f;

    private void Start()
    {
        foreach (var cam in cameras)
        {
            cam.enabled = false;
        }

        StartCoroutine(UpdateCameras());
    }

    IEnumerator UpdateCameras()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            foreach (var cam in cameras)
            {
                cam.enabled = true;
                cam.Render();
                cam.enabled = false;
            }
        }
    }
}
