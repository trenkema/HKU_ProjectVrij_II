using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStick : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    [SerializeField] int maxPapersBurned = 40;

    int papersBurned = 0;

    bool canBurn = true;

    private void PaperBurned()
    {
        papersBurned++;

        if (papersBurned >= maxPapersBurned)
        {
            particles.Stop();

            canBurn = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Destroyable"))
        {
            if (collision.transform.TryGetComponent(out PaperScript paperScript) && canBurn)
            {
                paperScript.SetInFlames();

                PaperBurned();
            }
        }
    }
}
