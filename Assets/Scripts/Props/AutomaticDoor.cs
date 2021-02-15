using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] private bool isLocked;
    [SerializeField] private float transitionTime;

    [SerializeField] private Transform leftDoorTransform;
    [SerializeField] private Transform rightDoorTransform;

    [SerializeField] private string resultName;
    bool isBound = false;
    int nEntities=0;

    private Vector2 leftClosedPosition, rightClosedPosition,leftOpenPosition, rightOpenPosition;

    private void Awake()
    {
        leftClosedPosition = leftDoorTransform.position;
        rightClosedPosition = rightDoorTransform.position;
        leftOpenPosition = (Vector2)leftDoorTransform.position + Vector2.left;
        rightOpenPosition = (Vector2)rightDoorTransform.position + Vector2.right;
        state = DoorState.Closed;

    }

    private void Start()
    {
        if (isLocked)
        {
            DialogueManager.instance.GetResultByName(resultName).OnTriggerResult += UnlockDoor;
            isBound = true;
        }
    }
    private enum DoorState
    {
        Open,
        Closed,
        Opening,
        Closing
    };
    DoorState state;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            nEntities++;
            if (!isLocked&&state != DoorState.Open && nEntities > 0)
            {
                state = DoorState.Opening;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            nEntities--;
            if (state != DoorState.Closed && nEntities <= 0)
            {
                state = DoorState.Closing;
            }
        }
    }


    private void Update()
    {
        switch (state)
        {
           
            case DoorState.Opening:
              leftDoorTransform.position += (Vector3)(Vector2.left *transitionTime*Time.deltaTime);

                rightDoorTransform.position += (Vector3)(Vector2.right * transitionTime * Time.deltaTime);

                if (Vector2.Distance(rightDoorTransform.position,rightOpenPosition) <= 0.01f
                    || (Vector2.Distance(leftDoorTransform.position, leftOpenPosition) <= 0.01f)){
                    state = DoorState.Open;
                }

                break;
            case DoorState.Closing:
                leftDoorTransform.position -= (Vector3)(Vector2.left * transitionTime * Time.deltaTime);

                rightDoorTransform.position -= (Vector3)(Vector2.right * transitionTime * Time.deltaTime);

                if (Vector2.Distance(rightDoorTransform.position, rightClosedPosition) <= 0.01f
                    || (Vector2.Distance(leftDoorTransform.position, leftClosedPosition) <= 0.01f))
                {
                    state = DoorState.Closed;
                }

                break;
        }
    }


    public void UnlockDoor()
    {
        if (isLocked)
        {
            isLocked = false;
            StartCoroutine(Unlocking());
        }
    }

    private IEnumerator Unlocking()
    {
        while(InitStateManager.currGameMode != GameModes.Powercut)
        {
            yield return null;
        }
        if (nEntities > 0)
            state = DoorState.Opening;
    }


    private void OnDestroy()
    {
        if (isBound)
        {
            DialogueManager.instance.GetResultByName(resultName).OnTriggerResult -= UnlockDoor;
        }
    }
}
