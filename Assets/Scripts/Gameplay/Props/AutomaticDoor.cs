using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] protected bool isLocked;
    [SerializeField] protected float transitionTime;

    [SerializeField] protected Animator animator;

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
        animator = GetComponent<Animator>();
        state = DoorState.Closed;
        animator.enabled = false;
    }

    protected void Start()
    {
        if (isLocked)
        {
            if (DialogueManager.instance != false)
            {
                DialogueManager.instance.GetResultByName(resultName).OnTriggerResult += UnlockDoor;
                isBound = true;
            }
        
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            nEntities++;
            if (!isLocked&&state != DoorState.Open && nEntities > 0)
            {
                if (state!= DoorState.Opening)
                {
                    animator.enabled = true;
                    animator.Play("Open");
                    state = DoorState.Opening;
                }
               
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
                if (state != DoorState.Closing)
                {
                    animator.enabled = true;
                    animator.Play("Close");
                    state = DoorState.Closing;
                }
            }
        }
    }

    public void OnOpen()
    {
        animator.enabled = false;
        state = DoorState.Open;
    }

    public void OnClose()
    {
        animator.enabled = false;
        state = DoorState.Closed;
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
        {
            animator.enabled = true;
            state = DoorState.Opening;
            animator.Play("Open");
        }
      
    }


    protected void OnDestroy()
    {
        if (isBound)
        {
            DialogueManager.instance.GetResultByName(resultName).OnTriggerResult -= UnlockDoor;
        }
    }
}
