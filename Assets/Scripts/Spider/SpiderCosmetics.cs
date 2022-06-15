using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpiderCosmetics : MonoBehaviour
{
    [SerializeField] GameObject[] hatCosmetics;

    [SerializeField] PhotonView PV;

    int hatInt = 0;

    private void Start()
    {
        foreach (var item in hatCosmetics)
        {
            item.SetActive(false);
        }

        if (PV.Owner.CustomProperties.ContainsKey("Hat"))
        {
            hatInt = (int)PV.Owner.CustomProperties["Hat"];
        }

        hatCosmetics[hatInt].SetActive(true);
    }
}
