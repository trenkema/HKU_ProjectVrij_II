using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LevelSettings : MonoBehaviourPunCallbacks
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI minutesText;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("Settings")]
    [SerializeField] int minMinutes, maxMinutes;

    [SerializeField] int minScoreNeeded, maxScoreNeeded;

    int currentMinutes;

    int currentScoreNeeded;

    Hashtable customLevelProperties = new Hashtable();

    public void ChangeMinutes(int _upDown)
    {
        currentMinutes += _upDown;

        currentMinutes = Mathf.Clamp(currentMinutes, minMinutes, maxMinutes);

        minutesText.text = string.Format("{0} minutes", currentMinutes);

        customLevelProperties["PlayTime"] = currentMinutes;

        PhotonNetwork.CurrentRoom.SetCustomProperties(customLevelProperties);
    }

    public void ChangeScoreNeeded(int _upDown)
    {
        currentScoreNeeded += _upDown;

        currentScoreNeeded = Mathf.Clamp(currentScoreNeeded, minScoreNeeded, maxScoreNeeded);

        scoreText.text = string.Format("First to {0}", currentScoreNeeded);

        customLevelProperties["ScoreNeeded"] = currentScoreNeeded;

        PhotonNetwork.CurrentRoom.SetCustomProperties(customLevelProperties);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PlayTime") && !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("ScoreNeeded"))
            {
                currentMinutes = minMinutes;
                currentScoreNeeded = minScoreNeeded;

                customLevelProperties["PlayTime"] = currentMinutes;
                customLevelProperties["ScoreNeeded"] = currentScoreNeeded;

                minutesText.text = string.Format("{0} minutes", currentMinutes);
                scoreText.text = string.Format("First to {0}", currentScoreNeeded);

                PhotonNetwork.CurrentRoom.SetCustomProperties(customLevelProperties);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(customLevelProperties);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("PlayTime"))
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                minutesText.text = string.Format("{0} minutes", (int)propertiesThatChanged["PlayTime"]);
            }
        }

        if (propertiesThatChanged.ContainsKey("ScoreNeeded"))
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                scoreText.text = string.Format("First to {0}", (int)propertiesThatChanged["ScoreNeeded"]);
            }
        }
    }
}
