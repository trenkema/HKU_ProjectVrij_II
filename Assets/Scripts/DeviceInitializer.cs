using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeviceInitializer : MonoBehaviour
{
    [SerializeField] GameObject[] mainMenuItemsVR;
    [SerializeField] GameObject[] mainMenuItemsNonVR;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.PHOTON_LOADED, PhotonLoaded);
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.PHOTON_LOADED, PhotonLoaded);
    }

    private void PhotonLoaded()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);
        bool isVR = xrDisplaySubsystems[0].running;

        if (isVR)
        {
            Debug.Log("VR Enabled");

            foreach (var item in mainMenuItemsVR)
            {
                item.SetActive(true);
            }
        }
        else
        {
            Debug.Log("VR Disabled");

            foreach (var item in mainMenuItemsNonVR)
            {
                item.SetActive(true);
            }
        }
    }
}
