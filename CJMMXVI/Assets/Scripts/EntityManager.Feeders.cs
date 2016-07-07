﻿using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityManager : MonoBehaviour
{
	#region Methods
	void UpdateFeeder(Feeder feeder, bool wasSelected)
	{
		if(feeder.feedTimer > 0.0f)
		{
			feeder.feedTimer -= Time.deltaTime;
			return;
		}

		bool shouldFeed =
			feeder.autoFeed ||
			(feeder.playerControlled && wasSelected);

		if(!shouldFeed) { return; }

		int thrown = Mathf.Max(1, gameData.man.breadThrown);
		for(int i = 0; i < thrown; ++i)
		{
			var food = foodPool.Get();
			AddEntity(food);
			food.transform.position =
				mallardSpawn.transform.position +
				Vector3.right * UE.Random.Range(-2.0f, 2.0f) +
				Vector3.forward * UE.Random.Range(0.0f, 4.0f);
			food.lifeTimer = food.decayDuration;
		}

		feeder.feedTimer = gameData.man.throwCooldown;
	}

	#endregion
}