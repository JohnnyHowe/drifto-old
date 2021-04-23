using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Update the track controller current section to the section that the car is on
/// </summary>
public class Car_TrackSectionUpdater : MonoBehaviour
{
    public TrackController trackController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.gameObject.tag == "Road")
        {
            trackController.currentSection = other.gameObject.transform.parent.gameObject.GetComponent<TrackSection>().number;
        }
    }
}
