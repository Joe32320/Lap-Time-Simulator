using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track  {

    string name;
    public List<TrackSection> sections;
    int trackLength;
    List<int> checkpoints;

    public Track(string nName, List<TrackSection> nSections)
    {
        name = nName;
        sections = nSections;
        checkpoints = new List<int>();
        trackLength = 0;

        for(int i = 0; i < sections.Count; i++)
        {
            sections[i].setStartPoint(trackLength);
            trackLength += sections[i].getLength();
            checkpoints.Add(trackLength);
            Debug.Log("Track Length: " + (trackLength) + "m");
        }

        Debug.Log("Track Length: " + (trackLength) + "m");

    }

    //Gets the section of the track the car is currently on based on current distance travelled.
    public TrackSection getCheckpointPassed(int distance)
    {
        int checkpoint = 0;

        for(int i = 0; i < checkpoints.Count; i++)
        {
            if(distance >= checkpoints[i])
            {
                checkpoint = (i+1)%checkpoints.Count;
            }
        }

        //if()


        return sections[checkpoint];
    }

    public int getLength()
    {
        return trackLength;
    }


        



}
