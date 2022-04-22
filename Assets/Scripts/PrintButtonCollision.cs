using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButtonCollision : MonoBehaviour
{
    [SerializeField] Transform paperSpawnTransform;

    [SerializeField] int paperAmount;

    [SerializeField] float spawnDelay = 0.25f;

    [SerializeField] float minPaperForce, maxPaperForce;

    public GameObject paperPrefab;

    bool hasPrinted = false;

    private IEnumerator SpawnPapers()
    {
        for (int i = 0; i < paperAmount; i++)
        {
            GameObject paper = Instantiate(paperPrefab, paperSpawnTransform.position, paperSpawnTransform.rotation);

            Rigidbody rb = paper.GetComponent<Rigidbody>();

            transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            float speed = Random.Range(minPaperForce, maxPaperForce);
            rb.isKinematic = false;
            Vector3 force = transform.forward;
            force = new Vector3(force.x, 1, force.z);
            paper.GetComponent<Rigidbody>().AddForce(force * speed);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasPrinted)
        {
            if (collision.collider == GameManager.Instance.leftHandPalmCollider || collision.collider == GameManager.Instance.rightHandPalmCollider)
            {
                hasPrinted = true;

                StartCoroutine(SpawnPapers());
            }
        }
    }
}
