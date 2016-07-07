using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct GameData
{
	#region Types
	[Serializable]
	public struct FeederData
	{
		[SerializeField]
		public int breadThrown;
		[SerializeField]
		public float throwCooldown;
		[SerializeField]
		public float airTime;
		[SerializeField]
		public int ammo;
		[SerializeField]
		public float range;
		[SerializeField]
		public float spread;
	}

	[Serializable]
	public struct MallardData
	{
		[SerializeField]
		public int count;
		[SerializeField]
		public float eatDuration;
		[SerializeField]
		public float speed;
		[SerializeField]
		public float acceleration;
		[SerializeField]
		public float rotationSpeed;
		[SerializeField]
		public int pointsPerQuack;
	}

	[Serializable]
	public struct EnvData
	{
	}
	#endregion

	#region Fields
	[SerializeField]
	public int points;
	[SerializeField]
	public FeederData man;
	[SerializeField]
	public MallardData mallard;
	[SerializeField]
	public EnvData environment;

	[SerializeField]
	public FeederData npcFeeders;
	[SerializeField]
	public FeederData manualFeeders;
	[SerializeField]
	public FeederData autoFeeders;
	#endregion

	#region Methods
	public void Limit()
	{
		LimitFeederData(ref man);
		LimitFeederData(ref npcFeeders);
		LimitFeederData(ref manualFeeders);
		LimitFeederData(ref autoFeeders);

		mallard.count = Mathf.Max(0, mallard.count);
		mallard.eatDuration = Mathf.Max(0.0001f, mallard.eatDuration);
		mallard.speed = Mathf.Max(0.01f, mallard.speed);
		mallard.acceleration = Mathf.Max(0.0f, mallard.acceleration);
		mallard.rotationSpeed = Mathf.Max(0.0f, mallard.rotationSpeed);
		mallard.pointsPerQuack = Mathf.Max(0, mallard.pointsPerQuack);
	}

	void LimitFeederData(ref FeederData data)
	{
		data.breadThrown = Mathf.Max(0, data.breadThrown);
		data.throwCooldown = Mathf.Max(0.0f, data.throwCooldown);
		data.airTime = Mathf.Max(0.0f, data.airTime);
		data.ammo = Mathf.Max(0, data.ammo);
		data.range = Mathf.Max(0.0f, data.range);
		data.spread = Mathf.Max(0.0f, data.spread);
	}
	#endregion
}
