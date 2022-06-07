using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Initialize(Player _player)
    {
        playerNameText.text = _player.NickName;
    }
}
