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
	public struct ManData
	{
		[SerializeField]
		public int breadThrown;
		[SerializeField]
		public float throwCooldown;
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
	public ManData man;
	[SerializeField]
	public MallardData mallard;
	[SerializeField]
	public EnvData environment;
	#endregion

	#region Methods
	#endregion
}
