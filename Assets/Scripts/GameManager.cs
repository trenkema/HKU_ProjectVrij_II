using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTypes { Human, Spiders }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Collider leftHandPalmCollider;
    public Collider rightHandPalmCollider;

    public bool isVR;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
