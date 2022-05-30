using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RopeManager : MonoBehaviour
{
    [SerializeField] Transform target;

    [SerializeField] List<GameObject> ropePoints = new List<GameObject>();

    [SerializeField] string ropePrefabName;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Setup(Vector3 _endPointTransform, Transform _target)
    {
        target = _target;

        EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, true);

        // Create an endpoint to attach the joint to
        GameObject endPoint = PhotonNetwork.Instantiate("EndPoint", _endPointTransform, Quaternion.identity);

        PV.RPC("RPC_SyncRope", RpcTarget.All, target.GetComponent<PhotonView>().ViewID, ropePoints[0].GetComponent<PhotonView>().ViewID, 
            ropePoints[ropePoints.Count - 1].GetComponent<PhotonView>().ViewID, endPoint.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    public void RPC_SyncRope(int _targetID, int _firstRopePointID, int _lastRopePointID, int _endPointID)
    {
        Transform target = PhotonView.Find(_targetID).transform;
        Transform firstRopePoint = PhotonView.Find(_firstRopePointID).transform;
        Transform lastRopePoint = PhotonView.Find(_lastRopePointID).transform;
        Transform endPoint = PhotonView.Find(_endPointID).transform;

        firstRopePoint.GetComponent<HingeJoint>().connectedBody = endPoint.GetComponent<Rigidbody>();

        target.SetParent(lastRopePoint);

        target.GetComponent<Rigidbody>().isKinematic = true;
    }
}
