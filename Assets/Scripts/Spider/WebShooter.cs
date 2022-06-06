using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class WebShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera spiderCamera;

    [SerializeField] Collider spiderCollider;

    [SerializeField] string webTrailPrefabName;

    [SerializeField] Transform webTrailSpawnPoint;

    [Header("Settings")]
    [SerializeField] float webTrailSpeed = 5f;

    [SerializeField] float shootDelay = 1f;

    bool canShoot = true;

    //SOUND

    private FMOD.Studio.EventInstance spiderShootSound;

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

                //SOUND

                spiderShootSound = FMODUnity.RuntimeManager.CreateInstance("event:/SpiderShoot");
                spiderShootSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                spiderShootSound.start();
            }
        }
    }

    IEnumerator DelayWebShooting()
    {
        yield return new WaitForSeconds(shootDelay);

        canShoot = true;
    }
}
