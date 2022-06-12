using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    [SerializeField] byte gameWonEventCode = 3;

    [SerializeField] TextMeshProUGUI scoreNeededText;

    [SerializeField] Transform leaderboardContainer;

    [SerializeField] GameObject leaderboardItemPrefab;

    Dictionary<Player, LeaderboardItem> leaderboardItems = new Dictionary<Player, LeaderboardItem>();

    int maxScore = 0;

    int ownScore = 0;

    public override void OnEnable()
    {
        base.OnEnable();

        EventSystemNew<Player, int>.Subscribe(Event_Type.UPDATE_SCORE, UpdateScore);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        EventSystemNew<Player, int>.Unsubscribe(Event_Type.UPDATE_SCORE, UpdateScore);
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddLeaderboardItem(player);
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("ScoreNeeded"))
        {
            maxScore = (int)PhotonNetwork.CurrentRoom.CustomProperties["ScoreNeeded"];

            scoreNeededText.text = string.Format("<color=#FF9F00>First to</color> <color=#18FF00>{0}</color> <color=#FF9F00>points</color>", maxScore);
        }
    }

    private void AddLeaderboardItem(Player _player)
    {
        LeaderboardItem item = Instantiate(leaderboardItemPrefab, leaderboardContainer).GetComponent<LeaderboardItem>();

        item.Initialize(_player);

        leaderboardItems.Add(_player, item);
    }

    private void RemoveLeaderboardItem(Player _player)
    {
        Destroy(leaderboardItems[_player].gameObject);

        leaderboardItems.Remove(_player);
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
    }
}
