using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    [SerializeField] string spherePrefabName;

    [Header("Raycast Settings")]

    [SerializeField] LayerMask raycastLayers;

    [SerializeField] float maxDistance = 1f;

    [Header("Rope Settings")]

    [SerializeField] Transform target;

    [SerializeField] GameObject spherePrefab;

    [SerializeField] Transform startPoint;

    [SerializeField] int amountOfPointsOffset = 0;

    [SerializeField] Vector3 ropeOffset = Vector3.zero;

    [SerializeField] float ropeSize = 0.25f;

    [SerializeField] float distanceBetweenPoints = 1f;

    List<GameObject> ropePoints = new List<GameObject>();

    List<GameObject> endPoints = new List<GameObject>();

    Vector3 instantiatePosition;

    float lerpValue = 0f;

    float lerpDistanceToAdd = 0f;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.COLLIDED, DestroyRope);
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.COLLIDED, DestroyRope);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!target.GetComponent<Spider>().IsGrounded())
            {
                RaycastRope();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (endPoints.Count != 0)
            {
                DestroyRope();
            }
        }
    }

    private void DestroyRope()
    {
        if (!PV.IsMine)
            return;

        EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, false);

        target.SetParent(null);

        target.GetComponent<Rigidbody>().isKinematic = false;

        //target.gameObject.SetActive(true);

        //foreach (var ropePoint in ropePoints)
        //{
        //    Destroy(ropePoint);
        //}

        //foreach (var endPoint in endPoints)
        //{
        //    Destroy(endPoint);
        //}

        foreach (var ropePoint in ropePoints)
        {
            PhotonNetwork.Destroy(ropePoint);
        }

        foreach (var endPoint in endPoints)
        {
            PhotonNetwork.Destroy(endPoint);
        }

        ropePoints.Clear();
    }

    private void RaycastRope()
    {
        RaycastHit rayHit;

        if (Physics.Raycast(startPoint.position, startPoint.forward, out rayHit, maxDistance, raycastLayers, QueryTriggerInteraction.Ignore))
        {
            GenerateRope(rayHit.point);
        }
    }

    private void GenerateRope(Vector3 _endPointTransform)
    {
        EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, true);

        // Create an endpoint to attach the joint to
        GameObject endPoint = new GameObject("Rope");

        endPoints.Add(endPoint);

        endPoint.AddComponent<Rigidbody>().isKinematic = true;

        endPoint.transform.position = _endPointTransform + ropeOffset;

        lerpValue = 0f;

        float distance = Vector3.Distance(startPoint.position, endPoint.transform.position);

        int amountOfPoints = (int)Mathf.Ceil(distance / distanceBetweenPoints);

        amountOfPoints -= amountOfPointsOffset;

        lerpDistanceToAdd = 1f / amountOfPoints;

        // Create all the points between the two vectors
        // i < amountOfPoints + 1
        for (int i = 0; i < amountOfPoints + 1; i++)
        {
            instantiatePosition = Vector3.Lerp(endPoint.transform.position, startPoint.position, lerpValue);

            //GameObject ropePoint = Instantiate(spherePrefab, instantiatePosition, Quaternion.identity, endPoint.transform);
            GameObject ropePoint = PhotonNetwork.Instantiate(spherePrefabName, instantiatePosition, Quaternion.identity);

            ropePoint.transform.SetParent(endPoint.transform);

            ropePoints.Add(ropePoint);

            ropePoint.transform.localScale = new Vector3(ropeSize, ropeSize, ropeSize);

            if (ropePoint.TryGetComponent(out HingeJoint joint))
            {
                if (i == 0)
                {
                    joint.connectedBody = endPoint.GetComponent<Rigidbody>();
                }
                else
                {
                    joint.connectedBody = ropePoints[i - 1].GetComponent<Rigidbody>();
                }

                // i == amountOfPoints - 1
                if (i == amountOfPoints)
                {
                    //target.gameObject.SetActive(false);

                    target.SetParent(ropePoint.transform);

                    target.GetComponent<Rigidbody>().isKinematic = true;
                }
            }

            lerpValue += lerpDistanceToAdd;
        }
    }
}
