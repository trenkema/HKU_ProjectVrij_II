using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public enum PlayerTypes { Human, Spiders }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] byte gameStartedEventCode = 6;
    [SerializeField] byte gameEndedEventCode = 7;

    [SerializeField] GameObject[] objectsToDisableForVR;
    [SerializeField] GameObject[] objectsToDisableForNonVR;

    [SerializeField] GameObject[] objectsToEnableForNonVROnStart;

    [SerializeField] GameObject playerWonHUDNonVR;
    [SerializeField] TextMeshProUGUI playerWonNameTextNonVR;

    [SerializeField] GameObject playerWonHUDVR;
    [SerializeField] TextMeshProUGUI[] playerWonNameTextsVR;

    public Collider leftHandPalmCollider;
    public Collider rightHandPalmCollider;

    public bool isVR;

    public bool gameStarted { private set; get; }

    public bool gameEnded { private set; get; }

    private void OnEnable()
    {
        EventSystemNew<string>.Subscribe(Event_Type.GAME_WON, GameWon);

        EventSystemNew.Subscribe(Event_Type.GAME_STARTED, GameStarted);

        EventSystemNew.Subscribe(Event_Type.GAME_ENDED, GameEnded);
    }

    private void OnDisable()
    {
        EventSystemNew<string>.Unsubscribe(Event_Type.GAME_WON, GameWon);

        EventSystemNew.Unsubscribe(Event_Type.GAME_STARTED, GameStarted);

        EventSystemNew.Unsubscribe(Event_Type.GAME_ENDED, GameEnded);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (isVR)
        {
            foreach (var item in objectsToDisableForVR)
            {
                Destroy(item);
            }
        }
        else
        {
            foreach (var item in objectsToDisableForNonVR)
            {
                Destroy(item);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            object[] content = new object[] { };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(gameStartedEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void StartGame()
    {
        object[] content = new object[] { };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(gameStartedEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void GameStarted()
    {
        gameStarted = true;
    }

    private void GameWon(string _playerName)
    {
        if (isVR)
        {
            playerWonHUDVR.SetActive(true);

            foreach (var item in playerWonNameTextsVR)
            {
                item.text = _playerName;
            }
        }
        else
        {
            playerWonHUDNonVR.SetActive(true);
            playerWonNameTextNonVR.text = _playerName;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void GameEnded()
    {
        gameStarted = false;

        gameEnded = true;
    }
}
