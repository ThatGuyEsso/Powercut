using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirConditionTask : BaseTask
{
    [SerializeField] private Color fixedColor;
    [SerializeField] private Color fixingColor;
    [SerializeField] private ChargingCable fixingCable;
    private Transform playerTransform;
    override protected void EvaluateSpriteDisplay()
    {
        if (isFixed) gfx.sprite = stateSprites[0];
        else if( isFixing) gfx.sprite = stateSprites[1];
        else gfx.sprite = stateSprites[2];
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.gameObject.CompareTag("Player") && !isFixed)
        {
            fixingCable.ChangeColour(fixingColor);
            playerTransform = other.transform;

        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.gameObject.CompareTag("Player") && fixingCable.isDrawing) fixingCable.StopDrawingRope();
        playerTransform = null;
    }


    protected override void Update()
    {
        base.Update();

        if(isFixed && fixingCable.isDrawing) fixingCable.ChangeColour(fixedColor);
    }

    public override void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && inRange)
        {
            switch (GameStateManager.instance.GetCurrentGameState())
            {

                //If Main power is of player can begin to fix 
                case GameStates.MainPowerOff:
                    //Begin fixing
                    isFixing = true;
                    Debug.Log("Should begin fixing");
                    InGamePrompt.instance.SetColor(Color.green);
                    InGamePrompt.instance.ShowPromptTimer(fixingPrompt, 5.0f);

                    if (playerTransform != false) fixingCable.StartDrawingRope(playerTransform);


                    break;

                default:
                    InGamePrompt.instance.SetColor(Color.red);
                    InGamePrompt.instance.ShowPromptTimer(powerStillOnPrompt, 5.0f);

                    break;
            }
        }
    }
}
