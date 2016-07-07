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
}
