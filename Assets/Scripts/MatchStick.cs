using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Destroyable"))
        {
            if (collision.transform.TryGetComponent(out PaperScript paperScript))
            {
                paperScript.SetInFlames();
            }
        }
    }
}
