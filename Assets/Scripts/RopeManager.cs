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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PV.IsMine)
            {
                ropePoints[ropePoints.Count - 1].GetComponent<Rigidbody>().AddForce(0, 0, 100);
            }
        }
    }

    public void Setup(Transform _target, Transform _endPoint)
    {
        if (!PV.IsMine)
            return;

        target = _target;

        EventSystemNew<bool>.RaiseEvent(Event_Type.IS_SWINGING, true);

        //ropePoints[0].GetComponent<HingeJoint>().connectedBody = _endPoint.GetComponent<Rigidbody>();

        //_target.SetParent(ropePoints[ropePoints.Count - 1].transform);

        //_target.GetComponent<Rigidbody>().isKinematic = true;

        PV.RPC("RPC_SyncRope", RpcTarget.All, target.GetComponent<PhotonView>().ViewID, ropePoints[0].GetComponent<PhotonView>().ViewID,
            ropePoints[ropePoints.Count - 1].GetComponent<PhotonView>().ViewID, _endPoint.GetComponent<PhotonView>().ViewID);
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
