using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [SerializeField] GameObject[] trackSections;
    [SerializeField] TrackSection startSection;

    public int generateSections = 10;

    TrackSection lastSection;
    void Start()
    {
        lastSection = startSection;
        for (int i = 0; i < generateSections; i ++) {
            CreateNewSection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateNewSection() {
        GameObject newTrack = Instantiate(ChooseSection(), transform);
        TrackSection newSection = newTrack.GetComponent<TrackSection>();

        Vector3 rotation = lastSection.endPosition.eulerAngles - newSection.startPosition.eulerAngles;
        newTrack.transform.eulerAngles = new Vector3(0, rotation.y, 0);
        Vector3 verticalRotation = lastSection.endPosition.eulerAngles - newSection.startPosition.eulerAngles;
        newTrack.transform.Rotate(verticalRotation, Space.Self);

        newTrack.transform.position = lastSection.endPosition.position - newSection.startPosition.position;
        lastSection = newSection;
    }

    GameObject ChooseSection() {
        float i = Random.Range(0, trackSections.Length);
        int index = 0;
        if (i != trackSections.Length) {
            index = Mathf.FloorToInt(i);
        }
        return trackSections[index];
    }
}
