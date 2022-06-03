using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [Header("Setup")]
    [SerializeField] string mainMenuScene;

    [SerializeField] string playerPrefab;
    [SerializeField] string spiderPrefab;

    [SerializeField] Transform[] playerSpawnPoints;
    [SerializeField] Transform[] spiderSpawnPoints;

    GameObject spawnedPlayer;

    bool hasLeft = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances(xrDisplaySubsystems);
            bool isVR = xrDisplaySubsystems[0].running;

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