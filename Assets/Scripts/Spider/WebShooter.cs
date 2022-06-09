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

    bool canShoot = true;

    public void ShootWeb(InputAction.CallbackContext _context)
    {
        if (_context.phase == InputActionPhase.Started)
        {
            if (canShoot)
            {
                canShoot = false;

                GameObject webTrail = PhotonNetwork.Instantiate(webTrailPrefabName, webTrailSpawnPoint.position, spiderCamera.transform.rotation);

                webTrail.GetComponent<WebTrail>().Setup(spiderCollider);
                webTrail.GetComponent<Rigidbody>().velocity = spiderCamera.transform.forward * webTrailSpeed;

                StartCoroutine(DelayWebShooting());
            }
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
