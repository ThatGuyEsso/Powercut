using System;

using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackList 
{
    [SerializeField] private float minTimeBtwnTrack, maxTimeBtwnTrack;

    //May create specific music class later
    [SerializeField] private List<Sound> trackList;

    private int trackIndex=0;

    public  void IncrementTrackIndex()
    {
        trackIndex++;

        //if out of range loop around
        if (trackIndex >= trackList.Count) trackIndex = 0;
    }

    public float GetTrackWaitTime() { return UnityEngine.Random.Range(minTimeBtwnTrack, maxTimeBtwnTrack); }
    public Sound StartTrackList()
    {
        trackIndex = 0;
        return trackList[0];
    }

    public Sound GetNextTrack()
    {
        int index = trackIndex + 1;

        //if out of range loop around
        if (index >= trackList.Count) index = 0;
        return trackList[index];
    }

    public Sound PlayNextTrack()
    {
        trackIndex++;

        //if out of range loop around
        if (trackIndex >= trackList.Count) trackIndex = 0;
      
        return trackList[trackIndex];
    }

    public void ResetRecord() { trackIndex = 0; }

}
