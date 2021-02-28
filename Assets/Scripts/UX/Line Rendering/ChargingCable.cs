using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingCable : MonoBehaviour
{
    private LineRenderer cable;//Line renderer reference
    public Transform origin;
    public RopeAnim ropeAnim;
    private Vector3 currentPoint;
    private  Transform targetTransform;
    private Vector3 lastTargetPosistion;
    [HideInInspector]
    public bool isDrawing;
    private bool isReeledIn;

    public float lineVelocity;
    public int lineQuality;
    public float damper;
    public float strength;
    public float waveHeight;
    public float waveCount;
    public float lerpSpeed;
    private float maxLerpSpeed;
    public AnimationCurve effectCurve;
    public void Awake()
    {
        //Cache References
        cable = gameObject.GetComponent<LineRenderer>();
        maxLerpSpeed = lerpSpeed;
        //Set up initial variables
        cable.positionCount = 0;

        currentPoint = origin.position;
        ropeAnim = new RopeAnim();
    }

    public void StartDrawingRope( Transform targetTrans)
    {
        currentPoint = origin.position;
        ChangeColour(Color.yellow);
        targetTransform = targetTrans;
        isDrawing = true;
        isReeledIn = false;
        StartCoroutine(IncreaseLerpSpeed());
    }

    public void StopDrawingRope()
    {
        isDrawing = false;
        if (targetTransform != null)
        {
            lastTargetPosistion = targetTransform.position;
            targetTransform = null;

        }

        currentPoint = lastTargetPosistion;
        lerpSpeed = maxLerpSpeed;
    }

    public void LateUpdate()
    {
        if (isDrawing)
        {

            DrawRope();
        }

        if(!isDrawing && !isReeledIn)
        {
            ReelRopeBackIn();
        }
    }

    private void DrawRope()
    {
        if (cable.positionCount == 0)
        {
            ropeAnim.SetVelocity(lineVelocity);

            cable.positionCount = lineQuality + 1;//Start and end
        }

        //Set up ropeanim settings
        ropeAnim.SetDamper(damper);
        ropeAnim.SetStrength(strength);
        ropeAnim.Update(Time.deltaTime);

        var right = Quaternion.LookRotation((targetTransform.position - origin.position).normalized) * Vector2.up;// get relative right direction
        currentPoint = Vector3.Lerp(currentPoint, targetTransform.position, Time.deltaTime * lerpSpeed);// animate rope shooting


        for (int i = 0; i < lineQuality + 1; i++)
        {
            var delta = i / (float)lineQuality;
            var offset = right * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * ropeAnim.Value * effectCurve.Evaluate(delta));
            cable.SetPosition(i, Vector3.Lerp(origin.position, currentPoint, delta) + offset);
        }
    }

    IEnumerator IncreaseLerpSpeed()
    {
        yield return new WaitForSeconds(0.5f);
        lerpSpeed *= 5;
    }

    public void ChangeColour(Color newColour)
    {
        cable.startColor=newColour;
        cable.endColor = newColour;
    }

    public void ReelRopeBackIn()
    {
        lerpSpeed = maxLerpSpeed;
        if (cable.positionCount == 0)
        {
            ropeAnim.SetVelocity(lineVelocity);

            cable.positionCount = lineQuality + 1;//Start and end
        }

        //Set up ropeanim settings
        ropeAnim.SetDamper(damper);
        ropeAnim.SetStrength(strength);
        ropeAnim.Update(Time.deltaTime);

        var right = Quaternion.LookRotation((currentPoint - origin.position).normalized) * Vector2.up;// get relative right direction
        currentPoint = Vector3.Lerp(currentPoint, origin.position, Time.deltaTime * lerpSpeed);// animate rope shooting


        for (int i = 0; i < lineQuality + 1; i++)
        {
            var delta = i / (float)lineQuality;
            var offset = right * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * ropeAnim.Value * effectCurve.Evaluate(delta));
            cable.SetPosition(i, Vector3.Lerp(origin.position, currentPoint, delta) + offset);
        }
        if (currentPoint == origin.position)
        {
            Debug.Log("Reeled back");
            isReeledIn = true;
            cable.positionCount = 0;
        }
    }
}
