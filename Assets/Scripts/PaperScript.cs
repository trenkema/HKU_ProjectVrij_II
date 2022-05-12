using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperScript : MonoBehaviour
{
    [SerializeField] float delayBeforeFlamable = 1f;

    [SerializeField] ParticleSystem flameParticles;

    bool isBurning = false;

    public float speed = 1.0f;
    public Color startColor;
    public Color endColor;
    [SerializeField] float burnTime = 4;

    float currentTime = 0;

    bool canBurn = false;

    private void Start()
    {
        Invoke("CanBurn", delayBeforeFlamable);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurning)
        {
            if (currentTime <= burnTime)
            {
                currentTime += Time.deltaTime;

                GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, currentTime / burnTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void CanBurn()
    {
        canBurn = true;
    }

    public void SetInFlames()
    {
        if (!isBurning && canBurn)
        {
            isBurning = true;

            flameParticles.Play();
        }
    }
}
