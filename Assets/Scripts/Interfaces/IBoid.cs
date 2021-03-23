using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoid 
{
	Transform GetTarget();
	Vector2 GetPosition();
	Vector2 GetVelocity();

	IBoid GetLeader();
	SteeringManager GetMovementManager();

	float GetArrivalRadius();
}
