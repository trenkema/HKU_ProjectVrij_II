using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    // HUDS
    [SerializeField] GameObject hudNonVR;
    [SerializeField] GameObject hudVR;

    // Non-VR Texts
    [SerializeField] TMP_InputField roomNameInputFieldNonVR;
    [SerializeField] TextMeshProUGUI roomNameTextNonVR;
    [SerializeField] TextMeshProUGUI errorTextNonVR;

    // VR Texts
    [SerializeField] TextMeshProUGUI roomNameTextVR;
    [SerializeField] TextMeshProUGUI errorTextVR;

    [SerializeField] TMP_InputField playerNameField;

    [SerializeField] GameObject[] mainMenuItemsToLoadVR;
    [SerializeField] GameObject[] mainMenuItemsToLoadNonVR;

    // Non-VR Menu's
    [SerializeField] GameObject mainMenuNonVR;
    [SerializeField] GameObject roomMenuNonVR;
    [SerializeField] GameObject errorMenuNonVR;
    [SerializeField] GameObject loadingMenuNonVR;

    // VR Menu's
    [SerializeField] GameObject mainMenuVR;
    [SerializeField] GameObject roomMenuVR;
    [SerializeField] GameObject errorMenuVR;
    [SerializeField] GameObject loadingMenuVR;

    [SerializeField] PanelManager panelManager;

    // Non-VR RoomList Stuff
    [SerializeField] GameObject roomListItemPrefabNonVR;
    [SerializeField] Transform roomListContentNonVR;

    // VR RoomList Stuff
    [SerializeField] GameObject roomListItemPrefabVR;
    [SerializeField] Transform roomListContentVR;

    // Non-VR PlayerList Stuff
    [SerializeField] GameObject playerListItemPrefabNonVR;
    [SerializeField] Transform playerListContentNonVR;

    // VR PlayerList Stuff
    [SerializeField] GameObject playerListItemPrefabVR;
    [SerializeField] Transform playerListContentVR;

    // Non-VR Buttons
    [SerializeField] Button startGameButtonNonVR;
    [SerializeField] Button leaveGameButtonNonVR;

    // VR Buttons
    [SerializeField] Button startGameButtonVR;
    [SerializeField] Button leaveGameButtonVR;

    // Level Settings
    [Header("Level Settings Menu")]
    [SerializeField] GameObject[] settingsButtonsForMasterClientNonVR;
    [SerializeField] GameObject[] settingsButtonsForMasterClientVR;

    [SerializeField] int maxPlayersPerRoom = 2;

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
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (GameManager.Instance.isVR)
        {
            hudNonVR.SetActive(false);

            foreach (var item in mainMenuItemsToLoadVR)
            {
                item.SetActive(false);
            }

            foreach (var item in settingsButtonsForMasterClientVR)
            {
                item.SetActive(false);
            }
        }
        else
        {
            hudVR.SetActive(false);

            foreach (var item in mainMenuItemsToLoadNonVR)
            {
                item.SetActive(false);
            }

            foreach (var item in settingsButtonsForMasterClientNonVR)
            {
                item.SetActive(false);
            }
        }

        Debug.Log("Connecting To Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetNickName(string _newNickname)
    {
        PhotonNetwork.NickName = _newNickname;

        PlayerPrefs.SetString("NickName", _newNickname);
    }

    public void SetNickName(TMP_InputField _inputField)
    {
        if (_inputField.text == string.Empty)
        {
            _inputField.text = PhotonNetwork.NickName;
            return;
        }

        PhotonNetwork.NickName = _inputField.text;

        PlayerPrefs.SetString("NickName", _inputField.text);
    }

    public void GetNickName(TMP_InputField _inputField)
    {
        _inputField.text = PhotonNetwork.NickName;
    }

    public void ClearInputField(TMP_InputField _inputField)
    {
        _inputField.text = "";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        if (GameManager.Instance.isVR)
        {
            foreach (var item in mainMenuItemsToLoadVR)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in mainMenuItemsToLoadNonVR)
            {
                item.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("NickName"))
        {
            playerNameField.text = PlayerPrefs.GetString("NickName");
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        }
        else
        {
            playerNameField.text = "Player " + Random.Range(0, 100).ToString("000");
            PhotonNetwork.NickName = playerNameField.text;
        }
    }

    public void CreateRoom()
    {
        if (GameManager.Instance.isVR)
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions() { MaxPlayers = (byte)maxPlayersPerRoom, PublishUserId = true }, null);
            panelManager.CloseAllPanels();

            loadingMenuVR.SetActive(true);
        }
        else
        {
            if (string.IsNullOrEmpty(roomNameInputFieldNonVR.text))
            {
                return;
            }

            PhotonNetwork.CreateRoom(roomNameInputFieldNonVR.text, new RoomOptions() { MaxPlayers = (byte)maxPlayersPerRoom, PublishUserId = true }, null);
            panelManager.CloseAllPanels();

            loadingMenuNonVR.SetActive(true);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);

        if (GameManager.Instance.isVR)
        {
            loadingMenuVR.SetActive(true);
        }
        else
        {
            loadingMenuNonVR.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        panelManager.CloseAllPanels();

        if (GameManager.Instance.isVR)
        {
            var hash = PhotonNetwork.LocalPlayer.CustomProperties;

            hash["isVR"] = true;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            roomMenuVR.SetActive(true);

            roomNameTextVR.text = PhotonNetwork.CurrentRoom.Name;

            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform trans in playerListContentVR)
            {
                Destroy(trans);
            }

            foreach (Player player in players)
            {
                Instantiate(playerListItemPrefabVR, playerListContentVR).GetComponent<PlayerListItem>()?.SetUp(player);
            }

            leaveGameButtonVR.interactable = true;
            startGameButtonVR.interactable = true;
            startGameButtonVR.gameObject.SetActive(PhotonNetwork.IsMasterClient);

            foreach (var item in settingsButtonsForMasterClientVR)
            {
                item.SetActive(PhotonNetwork.IsMasterClient);
            }
        }
        else
        {
            roomMenuNonVR.SetActive(true);

            roomNameTextNonVR.text = PhotonNetwork.CurrentRoom.Name;

            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform trans in playerListContentNonVR)
            {
                Destroy(trans);
            }

            foreach (Player player in players)
            {
                Instantiate(playerListItemPrefabNonVR, playerListContentNonVR).GetComponent<PlayerListItem>()?.SetUp(player);
            }

            leaveGameButtonNonVR.interactable = true;
            startGameButtonNonVR.interactable = true;
            startGameButtonNonVR.gameObject.SetActive(PhotonNetwork.IsMasterClient);

            foreach (var item in settingsButtonsForMasterClientNonVR)
            {
                item.SetActive(PhotonNetwork.IsMasterClient);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButtonNonVR.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        panelManager.CloseAllPanels();

        if (GameManager.Instance.isVR)
        {
            errorMenuVR.SetActive(true);

            errorTextVR.text = "Room Creation Failed: " + message;
        }
        else
        {
            errorMenuNonVR.SetActive(true);

            errorTextNonVR.text = "Room Creation Failed: " + message;
        }
    }

    public void StartGame(string _levelName)
    {
        PhotonNetwork.LoadLevel(_levelName);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        PhotonNetwork.LeaveRoom();

        if (GameManager.Instance.isVR)
        {
            loadingMenuVR.SetActive(true);
        }
        else
        {
            loadingMenuNonVR.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        panelManager.CloseAllPanels();

        if (GameManager.Instance.isVR)
        {
            mainMenuVR.SetActive(true);
        }
        else
        {
            mainMenuNonVR.SetActive(true);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (GameManager.Instance.isVR)
        {
            // Clear Room List
            foreach (Transform trans in roomListContentVR)
            {
                Destroy(trans.gameObject);
            }

            // Create Room List
            foreach (var room in roomList)
            {
                if (room.RemovedFromList)
                {
                    continue;
                }

                Instantiate(roomListItemPrefabVR, roomListContentVR).GetComponent<RoomListItem>()?.SetUp(room);
            }
        }
        else
        {
            // Clear Room List
            foreach (Transform trans in roomListContentNonVR)
            {
                Destroy(trans.gameObject);
            }

            // Create Room List
            foreach (var room in roomList)
            {
                if (room.RemovedFromList)
                {
                    continue;
                }

                Instantiate(roomListItemPrefabNonVR, roomListContentNonVR).GetComponent<RoomListItem>()?.SetUp(room);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (GameManager.Instance.isVR)
        {
            Instantiate(playerListItemPrefabVR, playerListContentVR).GetComponent<PlayerListItem>()?.SetUp(newPlayer);
        }
        else
        {
            Instantiate(playerListItemPrefabNonVR, playerListContentNonVR).GetComponent<PlayerListItem>()?.SetUp(newPlayer);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (GameManager.Instance.isVR)
        {
            foreach (var item in settingsButtonsForMasterClientVR)
            {
                item.SetActive(PhotonNetwork.IsMasterClient);
            }

            startGameButtonVR.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }
        else
        {
            foreach (var item in settingsButtonsForMasterClientNonVR)
            {
                item.SetActive(PhotonNetwork.IsMasterClient);
            }

            startGameButtonNonVR.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
