using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityManager : MonoBehaviour
{
	#region Methods
	void UpdateFood(Food food)
	{
		if(food.airTimeRemaining > 0.0f)
		{
			food.airTimeRemaining -= Time.deltaTime;

			float a = 1.0f - Mathf.Clamp01(food.airTimeRemaining / food.airTimeDuration);

			Vector3 pos = Vector3.Lerp(food.startPos, food.targetPos, a);
			pos.y += breadCurve.Evaluate(a); 

			food.transform.position = pos;
			return;
		}

		food.PlayWaterRings();

		food.lifeTimer -= Time.deltaTime;
		if(food.lifeTimer <= 0.0f)
		{
			RemoveEntity(food);
			foodPool.Return(food);
		}
	}
	#endregion
}
