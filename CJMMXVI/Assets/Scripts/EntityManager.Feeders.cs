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

	static readonly List<MeshRenderer> REND_BUFFER = new List<MeshRenderer>();

	private void UpdateBar(Feeder feeder)
	{
		if (feeder.Bar != null)
		{
			GameData.FeederData feederData = FeederDataForKind(feeder.kind);
			var barScale = feeder.Bar.transform.localScale;
			barScale.x = Mathf.Lerp(1, 0, feeder.feedTimer / feederData.throwCooldown);
			feeder.Bar.transform.localScale = barScale;
		}
	}


	void UpdateFeeder(Feeder feeder, bool wasSelected)
	{
		Transform feederTrans = feeder.transform;

		GameData.FeederData feederData = FeederDataForKind(feeder.kind);

		if (feeder.playerControlled && feeder.ammo == 0 && feeder.usesAmmo)
		{
			if (wasSelected)
			{
				// TODO: TEmp
				REND_BUFFER.Clear();
				feeder.GetComponentsInChildren<MeshRenderer>(REND_BUFFER);
				foreach (var rend in REND_BUFFER)
				{
					rend.material.color = Color.white;
				}
				feeder.ammo = feederData.ammo;

				feeder.ReloadCanvas.gameObject.SetActive(false);
			}
		}

		if (feeder.feedTimer > 0.0f)
		{
			feeder.feedTimer -= Time.deltaTime;

			UpdateBar(feeder);

			return;
		}

		if (feeder.Bar != null)
		{
			var barScale = feeder.Bar.transform.localScale;
			barScale.x = 1;
			feeder.Bar.transform.localScale = barScale;
		}

		if (feeder.throwDelayTimer > 0.0f)
		{
			feeder.throwDelayTimer -= Time.deltaTime;

			if(feeder.throwDelayTimer > 0.0f) { return; }

			int thrown = Mathf.Max(1, feederData.breadThrown);

			if(feeder.playerControlled && feeder.kind == Feeder.Kind.Player)
			{
				var playSfx = UE.Random.Range(0, 4) == 0;

				if(playSfx)
				{
					PlayRandomAudioFromList(sfx.OldManSfx);
				}
			}

			Vector3 feederFwd = feederTrans.forward;
			float spread = feederData.spread / 2.0f;
			float maxSpread = Mathf.Max(1.0f, feederData.range);

			Vector3 randomOrigin = feederTrans.position;
			randomOrigin.y = mallardSpawn.position.y;

			for(int i = 0; i < thrown; ++i)
			{
				var food = foodPool.Get();
				AddEntity(food);

				float range = UE.Random.Range(1.0f, maxSpread);
				float angle = UE.Random.Range(-spread, spread);
				var quat = Quaternion.Euler(0.0f, angle, 0.0f);

				Vector3 targetPos = randomOrigin + ((quat * feederFwd) * range);

				Vector3 startPos = feeder.feedOrigin.position;

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

			return;
		}

		

		bool shouldFeed =
			(!feeder.usesAmmo || feeder.ammo > 0) &&
			(feeder.autoFeed ||
			(feeder.playerControlled && wasSelected));

		if(!shouldFeed) { return; }

		if(feeder.usesAmmo)
		{
			--feeder.ammo;

			// TODO: Temp!
			if(feeder.ammo == 0)
			{
				REND_BUFFER.Clear();
				feeder.GetComponentsInChildren<MeshRenderer>(REND_BUFFER);
				foreach(var rend in REND_BUFFER)
				{
					rend.material.color = Color.red;
				}

				if (feeder.ReloadCanvas != null)
				{
					feeder.ReloadCanvas.gameObject.SetActive(true);
				}
			}
		}

		if(feeder.animator != null)
		{
			if(feeder.animator.gameObject.activeInHierarchy)
			{
				feeder.animator.SetTrigger(THROW_HASH);
			}
		}

		if(feeder.additionalAnimator != null)
		{
			if(feeder.additionalAnimator.gameObject.activeInHierarchy)
			{
				feeder.additionalAnimator.SetTrigger(THROW_HASH);
			}
		}

		feeder.throwDelayTimer = feederData.throwAnimSpeed == 0.0f ?
			1.0f : feeder.throwDelayDuration / feederData.throwAnimSpeed;
	}

	#endregion
}
