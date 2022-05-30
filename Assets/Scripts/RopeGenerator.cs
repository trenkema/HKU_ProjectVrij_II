using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeGenerator : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    [SerializeField] string ropePrefabName;

    [SerializeField] string spherePrefabName;

    [Header("Raycast Settings")]

    [SerializeField] LayerMask raycastLayers;

    [SerializeField] float maxDistance = 1f;

    [Header("Rope Settings")]

    [SerializeField] Transform target;

    [SerializeField] Spider spiderScript;

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

    GameObject ropePrefab;

    GameObject endPoint;

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
            if (!spiderScript.IsGrounded())
            {
                RaycastRope();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            //if (endPoints.Count != 0)
            //{
            //    DestroyRope();
            //}

            DestroyRope();
        }
    }

    private void DestroyRope()
    {
        if (!PV.IsMine)
            return;

        EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, false);

        PV.RPC("RPC_SyncTarget", RpcTarget.All, target.GetComponent<PhotonView>().ViewID, false);
        PV.RPC("RPC_UnParentTarget", RpcTarget.All, target.GetComponent<PhotonView>().ViewID);

        //target.SetParent(null);
        //target.GetComponent<Rigidbody>().isKinematic = false;

        //PhotonNetwork.Destroy(ropePrefab);

        //PhotonNetwork.Destroy(endPoint);

        ////PV.RPC("RPC_SyncTarget", RpcTarget.Others, target.GetComponent<PhotonView>().ViewID, false);

        ////target.gameObject.SetActive(true);

        ////foreach (var ropePoint in ropePoints)
        ////{
        ////    Destroy(ropePoint);
        ////}

        ////foreach (var endPoint in endPoints)
        ////{
        ////    Destroy(endPoint);
        ////}

        //if (ropePoints.Count > 0)
        //{
        //    foreach (var ropePoint in ropePoints)
        //    {
        //        PhotonNetwork.Destroy(ropePoint);
        //    }
        //}

        //if (endPoints.Count > 0)
        //{
        //    foreach (var endPoint in endPoints)
        //    {
        //        PhotonNetwork.Destroy(endPoint);
        //    }
        //}

        //ropePoints.Clear();

        //endPoints.Clear();
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
        //Vector3 targetDir = target.position - _endPointTransform;

        //Vector3 angle = Vector3.Angle(targetDir, target);

        endPoint = PhotonNetwork.Instantiate("EndPoint", _endPointTransform, Quaternion.identity);

        ropePrefab = PhotonNetwork.Instantiate(ropePrefabName, _endPointTransform, Quaternion.identity);

        ropePrefab.GetComponent<RopeManager>().Setup(target, endPoint.transform);

        //Vector3 newEndPointTransform = _endPointTransform + ropeOffset;

        //EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, true);

        //// Create an endpoint to attach the joint to
        //GameObject endPoint = PhotonNetwork.Instantiate("EndPoint", newEndPointTransform, Quaternion.identity);

        //endPoints.Add(endPoint);

        ////PV.RPC("RPC_SyncEndPoint", RpcTarget.Others, endPoint.GetComponent<PhotonView>().ViewID, newEndPointTransform.x, newEndPointTransform.y, newEndPointTransform.z);

        //lerpValue = 0f;

        //float distance = Vector3.Distance(startPoint.position, endPoint.transform.position);

        //int amountOfPoints = (int)Mathf.Ceil(distance / distanceBetweenPoints);

        //amountOfPoints -= amountOfPointsOffset;

        //lerpDistanceToAdd = 1f / amountOfPoints;

        //// Create all the points between the two vectors
        //// i < amountOfPoints + 1
        //for (int i = 0; i < amountOfPoints + 1; i++)
        //{
        //    instantiatePosition = Vector3.Lerp(endPoint.transform.position, startPoint.position, lerpValue);

        //    //GameObject ropePoint = Instantiate(spherePrefab, instantiatePosition, Quaternion.identity, endPoint.transform);
        //    GameObject ropePoint = PhotonNetwork.Instantiate(spherePrefabName, instantiatePosition, Quaternion.identity);

        //    ropePoint.transform.SetParent(endPoint.transform);

        //    ropePoints.Add(ropePoint);

        //    ropePoint.transform.localScale = new Vector3(ropeSize, ropeSize, ropeSize);

        //    if (ropePoint.TryGetComponent(out HingeJoint joint))
        //    {
        //        if (i == 0)
        //        {
        //            joint.connectedBody = endPoint.GetComponent<Rigidbody>();

        //            //PV.RPC("RPC_SyncRopePoint", RpcTarget.Others, ropePoint.GetComponent<PhotonView>().ViewID, endPoint.GetComponent<PhotonView>().ViewID, ropeSize);
        //        }
        //        else
        //        {
        //            joint.connectedBody = ropePoints[i - 1].GetComponent<Rigidbody>();

        //            //PV.RPC("RPC_SyncRopePoint", RpcTarget.Others, ropePoint.GetComponent<PhotonView>().ViewID, ropePoints[i - 1].GetComponent<PhotonView>().ViewID, ropeSize);
        //        }

        //        // i == amountOfPoints - 1
        //        if (i == amountOfPoints)
        //        {
        //            //target.gameObject.SetActive(false);

        //            target.SetParent(ropePoint.transform);

        //            target.GetComponent<Rigidbody>().isKinematic = true;

        //            //PV.RPC("RPC_SyncTarget", RpcTarget.Others, target.GetComponent<PhotonView>().ViewID, true);
        //        }
        //    }

        //    lerpValue += lerpDistanceToAdd;
        //}
    }

    [PunRPC]
    public void RPC_SyncEndPoint(int _endPointID, float _x, float _y, float _z)
    {
        PhotonView.Find(_endPointID).transform.position = new Vector3(_x, _y, _z);
    }

    [PunRPC]
    public void RPC_SyncRopePoint(int _ropePointID, int _endPointID, float _ropeSize)
    {
        PhotonView.Find(_ropePointID).GetComponent<HingeJoint>().connectedBody = PhotonView.Find(_endPointID).GetComponent<Rigidbody>();

        PhotonView.Find(_ropePointID).transform.localScale = new Vector3(_ropeSize, _ropeSize, _ropeSize);
    }

    [PunRPC]
    public void RPC_SyncTarget(int _targetID, bool _isKinematic)
    {
        PhotonView.Find(_targetID).GetComponent<Rigidbody>().isKinematic = _isKinematic;
    }

    [PunRPC]
    public void RPC_UnParentTarget(int _targetID)
    {
        PhotonView.Find(_targetID).transform.SetParent(null);

        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(ropePrefab);

            PhotonNetwork.Destroy(endPoint);
        }
    }
}
