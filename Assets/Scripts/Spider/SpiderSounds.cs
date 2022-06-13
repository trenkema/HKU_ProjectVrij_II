using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SpiderSounds : MonoBehaviour
{
    // Web Trail
    private EventInstance trailSoundEffect;
    private EventInstance webImpactSoundEffect;

    private void OnEnable()
    {
        EventSystemNew<Sound_Type, GameObject, bool>.Subscribe(Event_Type.TRIGGER_SOUND, TriggerSound);
    }

    private void OnDisable()
    {
        EventSystemNew<Sound_Type, GameObject, bool>.Unsubscribe(Event_Type.TRIGGER_SOUND, TriggerSound);
    }

    private void TriggerSound(Sound_Type _soundType, GameObject _target, bool _isStarted)
    {
        // Web Trail
        if (_soundType == Sound_Type.WebImpact)
        {
            trailSoundEffect.stop(STOP_MODE.ALLOWFADEOUT);

            webImpactSoundEffect = FMODUnity.RuntimeManager.CreateInstance("event:/WebCollider");
            webImpactSoundEffect.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_target));
            webImpactSoundEffect.start();
        }

        if (_soundType == Sound_Type.WebTrail)
        {
            trailSoundEffect = FMODUnity.RuntimeManager.CreateInstance("event:/WebEffect");
            trailSoundEffect.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(_target));
            trailSoundEffect.start();
        }
    }
}
