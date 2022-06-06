using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;

public class Spectate : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook spectateCamera;
    [SerializeField] Camera cam;

    [SerializeField] GameObject spectateHUD;

    [SerializeField] TextMeshProUGUI spiderNameText;

    int spectateID = 0;

    //SmoothCamera spectateCamera;

    RopeGenerator[] spiders;

    bool isSpectating = false;

    private void OnEnable()
    {
        EventSystemNew<bool>.Subscribe(Event_Type.SPIDER_DIED, SpiderDied);

        EventSystemNew<bool>.Subscribe(Event_Type.SPIDER_RESPAWNED, SpiderRespawned);
    }

    private void OnDisable()
    {
        EventSystemNew<bool>.Unsubscribe(Event_Type.SPIDER_DIED, SpiderDied);

        EventSystemNew<bool>.Unsubscribe(Event_Type.SPIDER_RESPAWNED, SpiderRespawned);
    }

    private void SpiderDied(bool _ownDeath)
    {
        if (_ownDeath)
        {
            spectateHUD.SetActive(true);

            isSpectating = true;

            spiders = FindObjectsOfType<RopeGenerator>();
        }

        if (isSpectating)
        {
            if (spiders[spectateID] == null)
            {
                spiders = FindObjectsOfType<RopeGenerator>();

                if (spiders.Length > 0)
                {
                    if (spiders[spectateID] != null)
                    {
                        spectateCamera.Follow = spiders[spectateID].transform;
                        spectateCamera.LookAt = spiders[spectateID].transform;

                        spectateCamera.gameObject.SetActive(true);
                        cam.gameObject.SetActive(true);

                        spiderNameText.text = spiders[spectateID].GetComponent<PhotonView>().Controller.NickName;
                    }
                }
            }
            else
            {
                spectateCamera.Follow = spiders[spectateID].transform;
                spectateCamera.LookAt = spiders[spectateID].transform;

                spectateCamera.gameObject.SetActive(true);
                cam.gameObject.SetActive(true);

                spiderNameText.text = spiders[spectateID].GetComponent<PhotonView>().Controller.NickName;
            }
        }
    }

    private void SpiderRespawned(bool _ownRespawn)
    {
        if (_ownRespawn)
        {
            if (spectateCamera != null)
            {
                isSpectating = false;

                spectateID = 0;

                spectateCamera.gameObject.SetActive(false);
                cam.gameObject.SetActive(false);
            }

            spectateHUD.SetActive(false);
        }
        else
        {
            SpiderDied(false);
        }
    }

    private void SwitchSpectator(int _upDown)
    {
        if (isSpectating)
        {
            spectateID += _upDown;

            if (spectateID > PhotonNetwork.PlayerList.Length - 1)
            {
                spectateID = 0;
            }
            else if (spectateID < 0)
            {
                spectateID = PhotonNetwork.PlayerList.Length - 1;
            }

            SpiderDied(false);
        }
    }
}
