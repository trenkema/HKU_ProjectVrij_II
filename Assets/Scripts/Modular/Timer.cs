using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool triggerOnEarly = false;
    [SerializeField] private UnityEvent onTimerEnd = new UnityEvent();

    [SerializeField] private PhotonView PV;

    private void Start()
    {
        if (!PV.IsMine)
        {
            return;
        }

        StartCoroutine(StartTimer());
    }

    public void StopTimerEarly()
    {
        if (triggerOnEarly)
        {
            onTimerEnd?.Invoke();
        }

        StopAllCoroutines();
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(duration);

        onTimerEnd?.Invoke();
    }
}
