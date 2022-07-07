using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool triggerOnEarly = false;
    [SerializeField] private UnityEvent onTimerEnd = new UnityEvent();

    private void Start()
    {
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
