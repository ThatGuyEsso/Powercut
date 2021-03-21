using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "New Record")]
public class Record : ScriptableObject
{
    [SerializeField]
    //all tracklists that record carry
    public List<TrackList> records;
}
