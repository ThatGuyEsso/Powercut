using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] protected bool isLocked;
    [SerializeField] protected float transitionTime;

    [SerializeField] protected Transform leftDoorTransform;
    [SerializeField] protected Transform rightDoorTransform;
    [SerializeField] protected Vector2 moveAxis;

    [SerializeField] protected string resultName;
    protected bool isBound = false;
    protected int nEntities =0;

    protected enum DoorState
    {
        Open,
        Closed,
        Opening,
        Closing
    };
    protected DoorState state;

    protected Vector2 leftClosedPosition, rightClosedPosition,leftOpenPosition, rightOpenPosition;

    protected void Awake()
    {
        if (leftDoorTransform != false)
        {
            leftClosedPosition = leftDoorTransform.position;
            leftOpenPosition = (Vector2)leftDoorTransform.position - moveAxis;

        }
        if (rightDoorTransform != false)
        {
            rightClosedPosition = rightDoorTransform.position;
            rightOpenPosition = (Vector2)rightDoorTransform.position + moveAxis;

        }
        state = DoorState.Closed;

    }

    protected void Start()
    {
        if (isLocked)
        {
            DialogueManager.instance.GetResultByName(resultName).OnTriggerResult += UnlockDoor;
            isBound = true;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
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


    protected void OnTriggerExit2D(Collider2D other)
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


    virtual protected void Update()
    {
        switch (state)
        {
           
            case DoorState.Opening:
                leftDoorTransform.position += (Vector3)(-moveAxis * transitionTime*Time.deltaTime);

                rightDoorTransform.position += (Vector3)(moveAxis * transitionTime * Time.deltaTime);

                if (Vector2.Distance(rightDoorTransform.position,rightOpenPosition) <= 0.01f
                    || (Vector2.Distance(leftDoorTransform.position, leftOpenPosition) <= 0.01f)){
                    state = DoorState.Open;
                }

                break;
            case DoorState.Closing:
                leftDoorTransform.position -= (Vector3)(-moveAxis * transitionTime * Time.deltaTime);

                rightDoorTransform.position -= (Vector3)(moveAxis * transitionTime * Time.deltaTime);

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

    protected IEnumerator Unlocking()
    {
        while(InitStateManager.currGameMode != GameModes.Powercut)
        {
            yield return null;
        }
        if (nEntities > 0)
            state = DoorState.Opening;
    }


    protected void OnDestroy()
    {
        if (isBound)
        {
            DialogueManager.instance.GetResultByName(resultName).OnTriggerResult -= UnlockDoor;
        }
    }
}
