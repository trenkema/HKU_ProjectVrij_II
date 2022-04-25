using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintButtonCollision : MonoBehaviour
{
    [SerializeField] Animator topAnimator;

    [SerializeField] Transform paperSpawnTransform;

    [SerializeField] int paperAmount;

    [SerializeField] float spawnDelay = 0.2f;

    [SerializeField] float spawnInterval = 0.25f;

    [SerializeField] float minPaperForce, maxPaperForce;

    public GameObject paperPrefab;

    bool hasPrinted = false;

    private IEnumerator SpawnPapers()
    {
        yield return new WaitForSeconds(spawnDelay);

        EventSystemNew<int>.RaiseEvent(Event_Type.SET_OBJECTIVE, 1);

        for (int i = 0; i < paperAmount; i++)
        {
            GameObject paper = Instantiate(paperPrefab, paperSpawnTransform.position, paperSpawnTransform.rotation);

            Rigidbody rb = paper.GetComponent<Rigidbody>();

            transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 180), Random.Range(0, 360));
            float speed = Random.Range(minPaperForce, maxPaperForce);
            rb.isKinematic = false;
            Vector3 force = transform.forward;
            force = new Vector3(force.x, 0.5f, force.z);
            paper.GetComponent<Rigidbody>().AddForce(force * speed);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasPrinted)
        {
            if (collision.collider == GameManager.Instance.leftHandPalmCollider || collision.collider == GameManager.Instance.rightHandPalmCollider)
            {
                topAnimator.SetBool("isOpen", true);

                hasPrinted = true;

                StartCoroutine(SpawnPapers());
            }
        }
    }
}
