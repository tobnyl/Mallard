using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityManager : MonoBehaviour
{
	#region Methods
	void UpdateMallard(Mallard mallard)
	{
		if(mallard.eatTimer > 0.0f)
		{
			mallard.eatTimer -= Time.deltaTime;

			if(mallard.eatTimer <= 0.0f)
			{
				if(onMallardEat != null) { onMallardEat(mallard); }

				PlayRandomAudioFromList(sfx.DuckSfx);
			}

			return;
		}

		Transform mallardTrans = mallard.transform;
		Vector3 mallardPos = mallardTrans.position;

		bool targetFoodAlive = mallard.targetFood != null &&
			foods.Contains(mallard.targetFood);

		if(!targetFoodAlive)
		{
			Food closestFood = null;
			float closestDist = Mathf.Infinity;

			for(int i = 0; i < foods.Count; ++i)
			{
				Food food = foods[i];

				float distSqr = (food.transform.position - mallardPos).sqrMagnitude;
				if(distSqr < closestDist)
				{
					closestFood = food;
					closestDist = distSqr;
				}
			}

			mallard.targetFood = closestFood;
		}

		Vector3 targetPos = mallard.targetFood == null ?
			mallard.defaultPosition :
			mallard.targetFood.transform.position;

		if(mallardTrans.position == targetPos)
		{
			return;
		}

		Vector3 fromMallardToTarget = targetPos - mallardPos;
		Vector3 dir = fromMallardToTarget.normalized;

		var isAtRotationTarget = mallard.RotateToTarget(dir, gameData.mallard.rotationSpeed);

		if(!isAtRotationTarget)
		{
			mallard.Velocity = Vector3.zero;
		}
		else
		{
			// if vel < speed then vel += acc * dt
			if(mallard.Velocity.normalized != dir || mallard.Velocity.magnitude < gameData.mallard.speed)
			{
				mallard.Velocity += dir * gameData.mallard.acceleration * Time.deltaTime;
			}

			//mallard.Velocity += -mallard.Velocity.normalized * mallard.decel

			mallard.Velocity = Vector3.ClampMagnitude(mallard.Velocity, fromMallardToTarget.magnitude);

			mallardTrans.position += mallard.Velocity;

			float finalDist = Vector3.Distance(mallardTrans.position, targetPos);

			if(finalDist < 0.001f)
			{
				mallard.Velocity = Vector3.zero;

				if(mallard.targetFood != null)
				{
					//Debug.Log(mallardTrans.gameObject.name + " reached target food", this);
					// Reached food!
					RemoveEntity(mallard.targetFood);
					foodPool.Return(mallard.targetFood);
					mallard.eatTimer = gameData.mallard.eatDuration;
					mallard.targetFood = null;
				}
			}
		}
	}
	#endregion
}
