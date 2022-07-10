using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AddPoints : MonoBehaviour
{
    [SerializeField] private PhotonView PV;

    public void SetPoints(GameObject _gObj)
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (_gObj.TryGetComponent(out PointsToEarn pointsScript))
        {
            Player player = PhotonNetwork.LocalPlayer;

            int currentScore = 0;

            if (player.CustomProperties.ContainsKey("Score"))
            {
                currentScore = (int)player.CustomProperties["Score"];
            }

            currentScore += pointsScript.points;

            var hash = player.CustomProperties;

            hash["Score"] = currentScore;

            player.SetCustomProperties(hash);
        }
    }
}
