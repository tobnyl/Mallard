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
	GameData.FeederData FeederDataForKind(Feeder.Kind kind)
	{
		return kind == Feeder.Kind.Player ? gameData.man :
			kind == Feeder.Kind.Npc ? gameData.npcFeeders :
			kind == Feeder.Kind.Auto ? gameData.autoFeeders :
			gameData.manualFeeders;
	}

	void UpdateFeeder(Feeder feeder, bool wasSelected)
	{
		if(feeder.feedTimer > 0.0f)
		{
			feeder.feedTimer -= Time.deltaTime;
			return;
		}

		GameData.FeederData feederData = FeederDataForKind(feeder.kind);

		if(feeder.playerControlled && feeder.ammo == 0 && feeder.usesAmmo)
		{
			if(wasSelected)
			{
				feeder.ammo = feederData.ammo;
			}
		}

		bool shouldFeed =
			(!feeder.usesAmmo || feeder.ammo > 0) &&
			(feeder.autoFeed ||
			(feeder.playerControlled && wasSelected));

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

			Vector3 dir = (targetPos - startPos).normalized;
			food.transform.rotation = Quaternion.Euler(0.0f, dir.ToAngleXZ(), 0.0f);

			food.lifeTimer = food.decayDuration;
			food.originatedFrom = feeder;
		}

		feeder.feedTimer = feederData.throwCooldown;
	}

	#endregion
}
