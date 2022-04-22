using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperScript : MonoBehaviour
{
    [SerializeField] ParticleSystem flameParticles;

    bool isBurning = false;

    public float speed = 1.0f;
    public Color startColor;
    public Color endColor;
    [SerializeField] float burnTime = 4;

    float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (isBurning)
        {
            if (currentTime <= burnTime)
            {
                currentTime += Time.deltaTime * speed;

                GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, currentTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetInFlames()
    {
        if (!isBurning)
        {
            isBurning = true;

            flameParticles.Play();
        }
    }
}
