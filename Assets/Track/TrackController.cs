using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [SerializeField] GameObject[] leftTrackSections;
    [SerializeField] GameObject[] rightTrackSections;
    [SerializeField] GameObject[] straightSections;
    [SerializeField] TrackSection startSection;

    public float straightSectionChance = 0.5f;
    public int forwardBufferSections = 1;
    public int rearBufferSections = 1;
    public int currentSection = 0;
    private int currentGeneratedSections = 0;
    public int lastTurn = -1;  // -1 = left, 1 = right
    TrackSection lastSection;

    void Start()
    {
        lastSection = startSection;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (currentGeneratedSections < currentSection + forwardBufferSections)
        {
            CreateNewSection();
            currentGeneratedSections += 1;
            lastSection.number = currentGeneratedSections;

            if (currentGeneratedSections > forwardBufferSections + rearBufferSections)
            {
                Destroy(transform.GetChild(i).gameObject);
                i++;
            }
        }
    }

    void CreateNewSection()
    {
        GameObject newTrack = Instantiate(ChooseSection(), transform);
        TrackSection newSection = newTrack.GetComponent<TrackSection>();

        Vector3 rotation = lastSection.endPosition.eulerAngles - newSection.startPosition.eulerAngles;
        newTrack.transform.eulerAngles = new Vector3(0, rotation.y, 0);
        Vector3 verticalRotation = lastSection.endPosition.eulerAngles - newSection.startPosition.eulerAngles;
        newTrack.transform.Rotate(verticalRotation, Space.Self);

        newTrack.transform.position = lastSection.endPosition.position - newSection.startPosition.position;
        lastSection = newSection;
    }

    GameObject ChooseSection()
    {
        GameObject chosenSection;
        if (Random.Range(0, 99) / 99.0 < straightSectionChance)
        {
            chosenSection = RandomChoice(straightSections);
        }
        else
        {
            if (lastTurn < 0)
            {
                // last was left
                chosenSection = RandomChoice(rightTrackSections);
            }
            else
            {
                // last was right
                chosenSection = RandomChoice(leftTrackSections);
            }
            lastTurn *= -1;
        }
        return chosenSection;
    }

    GameObject RandomChoice(GameObject[] objects)
    {
        float i = Random.Range(0, objects.Length);
        int index = i == objects.Length ? 0 : (int)i;
        return objects[index];
    }
}
