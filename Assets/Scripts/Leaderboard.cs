using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform leaderboardContainer;

    [SerializeField] GameObject leaderboardItemPrefab;

    Dictionary<Player, LeaderboardItem> leaderboardItems = new Dictionary<Player, LeaderboardItem>();

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddLeaderboardItem(player);
        }
    }

    private void AddLeaderboardItem(Player _player)
    {
        LeaderboardItem item = Instantiate(leaderboardItemPrefab, leaderboardContainer).GetComponent<LeaderboardItem>();

        item.Initialize(_player);

        leaderboardItems[_player] = item;
    }

    private void RemoveLeaderboardItem(Player _player)
    {
        Destroy(leaderboardItems[_player]);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddLeaderboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveLeaderboardItem(otherPlayer);

        leaderboardItems.Remove(otherPlayer);
    }
}
