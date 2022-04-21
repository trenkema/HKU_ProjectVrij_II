using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButtonCollision : MonoBehaviour
{

    public GameObject paperPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hands"))
        {
            Debug.Log("Printen");
            Instantiate(paperPrefab, new Vector3(0.742f, 0.803f, 4.607f), Quaternion.identity);
        }
    }
}
