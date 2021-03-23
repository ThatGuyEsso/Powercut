using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoid 
{
	Transform GetTarget();
	Vector2 GetPosition();
	Vector2 GetVelocity();

	IBoid GetLeader();

	float GetRadius();
	float GetSightLength();
	float GetMaxSpeed();
	float GetBehindLength();
	Vector3 GeRightVector();
	SteeringManager GetMovementManager();

	float GetArrivalRadius();
}
