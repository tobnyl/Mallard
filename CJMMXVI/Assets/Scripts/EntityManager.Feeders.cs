using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityManager : MonoBehaviour
{
	#region Constants
	static readonly int THROW_HASH = Animator.StringToHash("Throw");
	#endregion

	#region Methods
	void UpdateFeeder(Feeder feeder, bool wasSelected)
	{
		if(feeder.feedTimer > 0.0f)
		{
			feeder.feedTimer -= Time.deltaTime;
			return;
		}

		GameData.FeederData feederData =
			feeder.kind == Feeder.Kind.Player ? gameData.man :
			feeder.kind == Feeder.Kind.Npc ? gameData.npcFeeders :
			gameData.autoFeeders;

		bool shouldFeed =
			feeder.autoFeed ||
			(feeder.playerControlled && wasSelected);

		if(!shouldFeed) { return; }

		if(feeder.animator != null)
		{
			feeder.animator.SetTrigger(THROW_HASH);
		}

		int thrown = Mathf.Max(1, feederData.breadThrown);

		if (feeder.playerControlled && feeder.kind == Feeder.Kind.Player)
		{
			var playSfx = UE.Random.Range(0, 4) == 0;

			if (playSfx)
			{
				PlayRandomAudioFromList(sfx.OldManSfx);
			}
		}

		for(int i = 0; i < thrown; ++i)
		{
			var food = foodPool.Get();
			AddEntity(food);

			Vector3 startPos = feeder.feedOrigin.position;
			Vector3 targetPos = mallardSpawn.transform.position +
				Vector3.right * UE.Random.Range(-2.0f, 2.0f) +
				Vector3.forward * UE.Random.Range(0.0f, 4.0f);

			food.airTimeRemaining = food.airTimeDuration = feederData.airTime;
			food.transform.position = startPos;
			food.startPos = startPos;
			food.targetPos = targetPos;

			food.lifeTimer = food.decayDuration;
			food.originatedFrom = feeder;
		}

		feeder.feedTimer = feederData.throwCooldown;
	}

	#endregion
}
