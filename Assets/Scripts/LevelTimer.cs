using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class LevelTimer : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI timeText;

    float startTime = 0;

    float timeRemaining = 0;

    bool timerIsRunning = false;

    bool gameOver = false;

    public override void OnEnable()
    {
        base.OnEnable();

        EventSystemNew.Subscribe(Event_Type.GAME_STARTED, StartTimer);

        EventSystemNew<string>.Subscribe(Event_Type.GAME_WON, GameWon);

        EventSystemNew<float, bool>.Subscribe(Event_Type.SYNC_TIMER, SyncTimer);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        EventSystemNew.Unsubscribe(Event_Type.GAME_STARTED, StartTimer);

        EventSystemNew<string>.Unsubscribe(Event_Type.GAME_WON, GameWon);

        EventSystemNew<float, bool>.Unsubscribe(Event_Type.SYNC_TIMER, SyncTimer);
    }

    private void Awake()
    {
        startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["PlayTime"];

        startTime *= 60;

        DisplayTime(startTime);
    }

    private void Update()
    {
        if (gameOver)
            return;

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;

                timerIsRunning = false;

                DisplayTime(timeRemaining);

                if (PhotonNetwork.IsMasterClient)
                {
                    // Sync Timer
                    object[] content = new object[] { timeRemaining, timerIsRunning };

                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

                    PhotonNetwork.RaiseEvent((int)Event_Code.SyncTimer, content, raiseEventOptions, SendOptions.SendReliable);

                    foreach (var player in PhotonNetwork.PlayerList)
                    {
                        if (player.CustomProperties.ContainsKey("isVR"))
                        {
                            // Game Won
                            object[] contentWon = new object[] { player.NickName };

                            RaiseEventOptions raiseEventOptionsWon = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

                            PhotonNetwork.RaiseEvent((int)Event_Code.GameWon, contentWon, raiseEventOptionsWon, SendOptions.SendReliable);
                        }
                    }
                }
            }
        }
    }

    private void DisplayTime(float _timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(_timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(_timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void StartTimer()
    {
        timerIsRunning = true;

        timeRemaining = startTime;
    }

    private void SyncTimer(float _timeRemaining, bool _timerIsRunning)
    {
        timeRemaining = _timeRemaining;
        timerIsRunning = _timerIsRunning;

        DisplayTime(timeRemaining);
    }

    private void GameWon(string _playerName)
    {
        gameOver = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            object[] content = new object[] { timeRemaining, timerIsRunning };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent((int)Event_Code.SyncTimer, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}
