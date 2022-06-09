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

    [SerializeField] GameObject[] objectsToDisableForVR;
    [SerializeField] GameObject[] objectsToDisableForNonVR;

    [SerializeField] GameObject[] objectsToEnableForNonVROnStart;

    [SerializeField] GameObject playerWonHUD;
    [SerializeField] TextMeshProUGUI playerWonNameText;

    public Collider leftHandPalmCollider;
    public Collider rightHandPalmCollider;

    public bool isVR;

    private void OnEnable()
    {
        EventSystemNew<string>.Subscribe(Event_Type.GAME_WON, GameWon);
    }

    private void OnDisable()
    {
        EventSystemNew<string>.Unsubscribe(Event_Type.GAME_WON, GameWon);
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

    private void GameWon(string _playerName)
    {
        playerWonHUD.SetActive(true);
        playerWonNameText.text = _playerName;
    }

    public void StartGame()
    {
        object[] content = new object[] { };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(gameStartedEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
