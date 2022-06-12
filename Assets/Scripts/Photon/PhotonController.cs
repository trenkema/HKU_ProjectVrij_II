using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonController : MonoBehaviour
{
    [SerializeField] GameObject cam;

    [SerializeField] Component[] componentsToDisable;

    [SerializeField] GameObject[] gameObjectsToDisable;

    [SerializeField] PhotonView PV;

    private void Start()
    {
        if (!PV.IsMine)
        {
            if (cam != null)
                Destroy(cam);

            foreach (var component in componentsToDisable)
            {
                Destroy(component);
            }

            foreach (var gObject in gameObjectsToDisable)
            {
                Destroy(gObject);
            }

            return;
        }
    }
}
