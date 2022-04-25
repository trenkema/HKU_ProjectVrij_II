using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    [SerializeField] int startHours;

    [SerializeField] int startMinutes;

    [SerializeField] TextMeshProUGUI timeText;

    float currentHours;

    float currentMinutes;

    float currentSeconds = 0;

    private void Start()
    {
        currentHours = startHours;
        currentMinutes = startMinutes;
    }

    private void Update()
    {
        currentSeconds += Time.deltaTime;

        if (currentSeconds >= 60f)
        {
            currentSeconds = 0;

            currentMinutes++;

            if (currentMinutes >= 60f)
            {
                currentMinutes = 0f;

                currentHours++;
            }

            if (currentHours >= 24f)
            {
                currentHours = 0f;
            }
        }

        timeText.text = string.Format("{0}:{1}", currentHours.ToString("00"), currentMinutes.ToString("00"));
    }
}
