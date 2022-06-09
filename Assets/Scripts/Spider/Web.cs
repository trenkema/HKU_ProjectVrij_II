using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{

    [SerializeField] int timeToDestroyWeb = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(timeToDestroyWeb);

        Destroy(this.gameObject);
    }
}
