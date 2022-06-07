using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [Header("Setup")]
    [SerializeField] byte respawnSpiderEventCode = 2;

    [SerializeField] TextMeshProUGUI respawnTimeText;
    [SerializeField] int respawnTime = 5;
    [SerializeField] int respawnTimeIncrease = 5;
    [SerializeField] int maxRespawnTime = 25;

    [SerializeField] string eventReceiverPrefabName;

    [SerializeField] string mainMenuScene;

    [SerializeField] string playerPrefab;
    [SerializeField] string spiderPrefab;

    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] spiderSpawnPoints;

    GameObject spawnedPlayer;

    bool hasLeft = false;

    int currentTime = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentTime = respawnTime;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;

        EventSystemNew<bool, bool>.Subscribe(Event_Type.SPIDER_DIED, SpiderDied);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

        EventSystemNew<bool, bool>.Unsubscribe(Event_Type.SPIDER_DIED, SpiderDied);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate(eventReceiverPrefabName, Vector3.zero, Quaternion.identity);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplaySubsystems);

            bool isVR = xrDisplaySubsystems.Count != 0 ? xrDisplaySubsystems[0].running : false;

            if (isVR)
            {
                int randomPlayerSpawnPoint = Random.Range(0, playerSpawnPoints.Length);

                spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab, playerSpawnPoints[randomPlayerSpawnPoint].position, Quaternion.identity);
            }
            else
            {
                int randomSpiderSpawnPoint = Random.Range(0, spiderSpawnPoints.Length);

                spawnedPlayer = PhotonNetwork.Instantiate(spiderPrefab, spiderSpawnPoints[randomSpiderSpawnPoint].position, Quaternion.identity);
            }
        }
    }

    private void SpiderDied(bool _ownDeath, bool _increaseRespawnTime)
    {
        if (_ownDeath)
        {
            StartCoroutine(RespawnSpider(_increaseRespawnTime));
        }
    }

    private IEnumerator RespawnSpider(bool _increaseRespawnTime)
    {
        respawnTimeText.text = string.Format("{0} Seconds", currentTime);

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);

            currentTime -= 1;

            respawnTimeText.text = string.Format("{0} Seconds", currentTime);
        }

        int randomSpiderSpawnPoint = Random.Range(0, spiderSpawnPoints.Length);

        spawnedPlayer = PhotonNetwork.Instantiate(spiderPrefab, spiderSpawnPoints[randomSpiderSpawnPoint].position, Quaternion.identity);

        object[] content = new object[] { spawnedPlayer.GetComponent<PhotonView>().ViewID, true };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(respawnSpiderEventCode, content, raiseEventOptions, SendOptions.SendReliable);

        if (_increaseRespawnTime)
        {
            if (respawnTime + respawnTimeIncrease <= maxRespawnTime)
            {
                respawnTime += respawnTimeIncrease;
            }
        }

        currentTime = respawnTime;

        yield break;
    }

    public void LeaveRoom()
    {
        if (!hasLeft)
        {
            hasLeft = true;

            if (spawnedPlayer != null)
                PhotonNetwork.Destroy(spawnedPlayer);

            PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}