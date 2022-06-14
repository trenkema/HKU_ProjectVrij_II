using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpiderCosmetics : MonoBehaviour
{
    [SerializeField] GameObject[] hatCosmetics;

    [SerializeField] PhotonView PV;

    private void Start()
    {
        foreach (var item in hatCosmetics)
        {
            item.SetActive(false);
        }

        int hatInt = (int)PV.Owner.CustomProperties["Hat"];

        hatCosmetics[hatInt].SetActive(true);
    }
}
