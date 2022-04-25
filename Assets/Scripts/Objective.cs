using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] GameObject printObjectiveUI;

    [SerializeField] GameObject cleanObjectiveUI;

    private void OnEnable()
    {
        EventSystemNew<int>.Subscribe(Event_Type.SET_OBJECTIVE, SetObjective);
    }

    private void OnDisable()
    {
        EventSystemNew<int>.Unsubscribe(Event_Type.SET_OBJECTIVE, SetObjective);
    }

    private void SetObjective(int _index)
    {
        switch (_index)
        {
            case 0:
                printObjectiveUI.SetActive(true);
                cleanObjectiveUI.SetActive(false);
                break;
            case 1:
                printObjectiveUI.SetActive(false);
                cleanObjectiveUI.SetActive(true);
                break;
        }
    }
}
