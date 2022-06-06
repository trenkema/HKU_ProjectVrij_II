using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWalk : MonoBehaviour
{
    private FMOD.Studio.EventInstance spiderRunningSound;
    // Start is called before the first frame update
    void Start()
    {
    }
    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }
    // Update is called once per frame
    void Update()
    {
        spiderRunningSound = FMODUnity.RuntimeManager.CreateInstance("event:/SpiderRun");
        spiderRunningSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

      //     if (Input.GetButtonDown("LeftShift")) spiderRunningSound.start();

      //      if (Input.GetButtonUp("LeftShift")) spiderRunningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }
}
