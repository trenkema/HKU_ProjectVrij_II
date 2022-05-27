using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonController : MonoBehaviour
{
    [SerializeField] GameObject cam;

    [SerializeField] Component[] componentsToDisable;

    [SerializeField] PhotonView PV;

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(cam);

            foreach (var component in componentsToDisable)
            {
                Destroy(component);
            }

            return;
        }
    }
}
