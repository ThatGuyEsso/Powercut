using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //static instance
    public static DialogueManager instance;

    //Contains all the results we want to process
    [SerializeField] private ResultData resultData;

    //beat to send to
    public static int nextBeat;

}
