using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : BaseSteering
{
    [SerializeField] private float tickRate;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float detectionArcAngle;
    [SerializeField] private int rayCount;
    [SerializeField] private float angleBetweenRays;
    [SerializeField] LayerMask detectionLayers;

    [SerializeField] private float avoidanceForce;

    Vector2 netForce;
    float currentAngle;

    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);
        if (isActive)
            StartCoroutine(CheckCollision());
        else
            StopAllCoroutines();
    }
    private IEnumerator CheckCollision()
    {
        netForce = Vector2.zero;
        currentAngle = (EssoUtility.GetAngleFromVector(transform.right) - detectionArcAngle / 2)+90;
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 point;
            //Using utility function to convert a angle into a vector
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, EssoUtility.GetVectorFromAngle(currentAngle), detectionRadius, detectionLayers);
            if (hitInfo)
            {
                //If it hits something the current vertex position = point
                point = hitInfo.point;
                AvoidPoint(point);
            }
            else
            {
                //If not just draw full length of ray in current angle
                point = transform.position + EssoUtility.GetVectorFromAngle(currentAngle) * detectionRadius;
            }
            Debug.DrawLine(transform.position, point, Color.green, 0.5f);
            currentAngle -= angleBetweenRays;
        }
      
        yield return new WaitForSeconds(tickRate);
        if (isActive) StartCoroutine(CheckCollision());
    }


    private void AvoidPoint(Vector2 point)
    {
        Vector2 avoidDir = (Vector2)transform.right - point;
        netForce+= avoidDir.normalized * avoidanceForce * Time.deltaTime;
    }
    public override Vector2 CalculateResultantForce()
    {
        return netForce;
    }
}
