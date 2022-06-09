using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartGame : MonoBehaviour
{
    [SerializeField] GameObject[] objectToDisableOnStart;
    [SerializeField] GameObject[] objectToEnableOnStart;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.GAME_STARTED, GameStarted);

        if (GameManager.Instance.gameStarted)
        {
            foreach (var item in objectToDisableOnStart)
            {
                item.SetActive(false);
            }

            foreach (var item in objectToEnableOnStart)
            {
                item.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.GAME_STARTED, GameStarted);
    }

    private void Awake()
    {
        foreach (var item in objectToEnableOnStart)
        {
            item.SetActive(false);
        }
    }

    private void GameStarted()
    {
        foreach (var item in objectToDisableOnStart)
        {
            item.SetActive(false);
        }

        foreach (var item in objectToEnableOnStart)
        {
            item.SetActive(true);
        }
    }
}
