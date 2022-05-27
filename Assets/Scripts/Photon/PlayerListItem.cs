using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Color ownPlayerTextColor;
    [SerializeField] TextMeshProUGUI playerNameText;
    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        playerNameText.text = _player.NickName;

        if (player == PhotonNetwork.LocalPlayer)
            playerNameText.color = ownPlayerTextColor;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
