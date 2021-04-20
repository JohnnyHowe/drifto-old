using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [SerializeField] GameObject[] trackSections;
    [SerializeField] TrackSection startSection;

    public int forwardBufferSections = 1;
    public int rearBufferSections = 1;
    public int currentSection = 0;
    private int currentGeneratedSections = 0;
    TrackSection lastSection;

    void Start() {
        lastSection = startSection;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (currentGeneratedSections < currentSection + forwardBufferSections) {
            CreateNewSection();
            currentGeneratedSections += 1;
            lastSection.number = currentGeneratedSections;

            if (currentGeneratedSections > forwardBufferSections + rearBufferSections) {
                Destroy(transform.GetChild(i).gameObject);
                i ++;
            }
        }
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
