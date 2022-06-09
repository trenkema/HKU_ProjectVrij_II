using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCollider : MonoBehaviour
{
    [SerializeField] GameObject hud;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hud.activeInHierarchy)
            {
                hud.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hud.SetActive(false);
        }
    }
}
