using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerTypes { Human, Spiders }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] GameObject[] objectsToDisableForVR;

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
    }

    private void GameWon(string _playerName)
    {
        playerWonHUD.SetActive(true);
        playerWonNameText.text = _playerName;
    }
}
