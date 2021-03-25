using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IBoid 
{
	Transform GetTarget();
	BaseEnemy GetGameObjectRef();
	Vector2 GetPosition();
	Vector2 GetVelocity();

	IBoid GetLeader();

	float GetRadius();
	float GetSightLength();
	float GetMaxSpeed();
	float GetBehindLength();
	Vector3 GeRightVector();
	SteeringManager GetMovementManager();
	void AddFollower(BaseEnemy newFollower);
	void FollowerDestroyedTarget();
	void FolowerDied(BaseEnemy follower);
	float GetArrivalRadius();

}
