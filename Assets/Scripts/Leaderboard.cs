using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    [SerializeField] byte gameWonEventCode = 3;

    [SerializeField] int maxScore = 3;

    [SerializeField] Transform leaderboardContainer;

    [SerializeField] GameObject leaderboardItemPrefab;

    Dictionary<Player, LeaderboardItem> leaderboardItems = new Dictionary<Player, LeaderboardItem>();

    int ownScore = 0;

    private void OnEnable()
    {
        EventSystemNew<Player, int>.Subscribe(Event_Type.UPDATE_SCORE, UpdateScore);
    }

    private void OnDisable()
    {
        EventSystemNew<Player, int>.Unsubscribe(Event_Type.UPDATE_SCORE, UpdateScore);
    }

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

    private void UpdateScore(Player _player, int _addToScore)
    {
        ownScore += _addToScore;

        leaderboardItems[_player].UpdateScore(ownScore);

        if (ownScore >= maxScore)
        {
            object[] content = new object[] { _player.NickName };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(gameWonEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
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
