using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float life = 3;
    public GameObject playerSpider;
    public GameObject webPrefab;

    void Start()
    {
        transform.Rotate(0, 90, 90);
    }
    void Awake()
    {
        Destroy(gameObject, life); 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            //Destroy(collision.gameObject);
            Instantiate(webPrefab, this.gameObject.transform.position, Quaternion.identity);
            
        }
    }

    public void Setup(Collider PlayerCollider)
    {
        Physics.IgnoreCollision(PlayerCollider, GetComponent<Collider>());
    }

}
