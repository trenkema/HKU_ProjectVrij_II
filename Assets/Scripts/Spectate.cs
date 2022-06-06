using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;

public class Spectate : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook spectateCamera;
    [SerializeField] CinemachineVirtualCamera noSpectatorsCamera;

    [SerializeField] Camera cam;

    [SerializeField] GameObject spectateHUD;

    [SerializeField] TextMeshProUGUI spiderNameText;

    int spectateID = 0;

    //SmoothCamera spectateCamera;

    List<RopeGenerator> spiders = new List<RopeGenerator>();

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
            spiders.Clear();

            spectateHUD.SetActive(true);

            isSpectating = true;

            RopeGenerator[] tempSpiders = FindObjectsOfType<RopeGenerator>();

            foreach (var spider in tempSpiders)
            {
                if (spider.GetComponent<PhotonView>().IsMine)
                {
                    break;
                }
                else
                {
                    spiders.Add(spider);
                }
            }
        }

        if (isSpectating)
        {
            if (spiders.Count > 0)
            {
                if (spiders[spectateID] == null)
                {
                    spiders.Clear();

                    RopeGenerator[] tempSpiders = FindObjectsOfType<RopeGenerator>();

                    foreach (var spider in tempSpiders)
                    {
                        if (spider.GetComponent<PhotonView>().IsMine)
                        {
                            break;
                        }
                        else
                        {
                            spiders.Add(spider);
                        }
                    }

                    if (spiders.Count > 0)
                    {
                        noSpectatorsCamera.gameObject.SetActive(false);

                        if (spiders[spectateID] != null)
                        {
                            spectateCamera.Follow = spiders[spectateID].transform;
                            spectateCamera.LookAt = spiders[spectateID].transform;

                            spectateCamera.gameObject.SetActive(true);
                            cam.gameObject.SetActive(true);

                            spiderNameText.text = spiders[spectateID].GetComponent<PhotonView>().Controller.NickName;
                        }
                    }
                    else
                    {
                        noSpectatorsCamera.gameObject.SetActive(true);
                        cam.gameObject.SetActive(true);

                        spiderNameText.text = "Nobody";
                    }
                }
                else
                {
                    noSpectatorsCamera.gameObject.SetActive(false);

                    spectateCamera.Follow = spiders[spectateID].transform;
                    spectateCamera.LookAt = spiders[spectateID].transform;

                    spectateCamera.gameObject.SetActive(true);
                    cam.gameObject.SetActive(true);

                    spiderNameText.text = spiders[spectateID].GetComponent<PhotonView>().Controller.NickName;
                }
            }
            else
            {
                noSpectatorsCamera.gameObject.SetActive(true);
                cam.gameObject.SetActive(true);

                spiderNameText.text = "Nobody";
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
                noSpectatorsCamera.gameObject.SetActive(false);
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
