using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TMPro;

public class WebShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera spiderCamera;

    [SerializeField] Collider spiderCollider;

    [SerializeField] string webTrailPrefabName;

    [SerializeField] Transform webTrailSpawnPoint;

    [SerializeField] TextMeshProUGUI webShootCooldownText;

    [Header("Settings")]
    [SerializeField] float webTrailSpeed = 5f;

    [SerializeField] int shootDelay = 1;

    //SOUND

    private FMOD.Studio.EventInstance spiderShootSound;

    bool canShoot = true;

    private void OnEnable()
    {
        EventSystemNew.Subscribe(Event_Type.Shoot, ShootWeb);
    }

    private void OnDisable()
    {
        EventSystemNew.Unsubscribe(Event_Type.Shoot, ShootWeb);
    }

    public void ShootWeb()
    {
        if (canShoot)
        {
            canShoot = false;

            GameObject webTrail = PhotonNetwork.Instantiate(webTrailPrefabName, webTrailSpawnPoint.position, spiderCamera.transform.rotation);

            webTrail.GetComponent<WebTrail>().Setup(spiderCollider);
            webTrail.GetComponent<Rigidbody>().velocity = spiderCamera.transform.forward * webTrailSpeed;

            StartCoroutine(DelayWebShooting());

            // SOUND
            spiderShootSound = FMODUnity.RuntimeManager.CreateInstance("event:/SpiderShoot");
            spiderShootSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            spiderShootSound.start();
        }
    }

    IEnumerator DelayWebShooting()
    {
        int currentTime = shootDelay;

        webShootCooldownText.text = currentTime.ToString();

        webShootCooldownText.gameObject.SetActive(true);

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);

            currentTime -= 1;

            webShootCooldownText.text = currentTime.ToString();
        }

        webShootCooldownText.gameObject.SetActive(false);

        canShoot = true;
    }
}
