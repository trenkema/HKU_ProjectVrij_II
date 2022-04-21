using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButtonCollision : MonoBehaviour
{
    public GameObject paperPrefab;

    bool hasPrinted = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!hasPrinted)
        {
            if (collision.collider == GameManager.Instance.leftHandPalmCollider || collision.collider == GameManager.Instance.rightHandPalmCollider)
            {
                hasPrinted = true;

                Debug.Log("Printen");
                Instantiate(paperPrefab, new Vector3(0.742f, 0.803f, 4.607f), Quaternion.identity);
            }
        }
    }
}
