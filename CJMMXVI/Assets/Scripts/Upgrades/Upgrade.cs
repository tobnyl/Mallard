using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Upgrade
{
	#region Types
	[Serializable]
	public class ManData
	{
		[SerializeField]
		public float throwCooldown;
	}

	[Serializable]
	public class MallardData
	{
		[SerializeField]
		public float eatDuration;
	}

	[Serializable]
	public class EnvData
	{
	}

	[Serializable]
	public class SceneData
	{
		[SerializeField]
		public Transform[] toEnable;
		[SerializeField]
		public Transform[] toDisable;
	}
	#endregion

	#region Fields
	[SerializeField]
	public int cost;

	[SerializeField]
	public float researchTime;

	[SerializeField]
	public ManData man;

	[SerializeField]
	public MallardData mallard;

	[SerializeField]
	public EnvData environment;

	[SerializeField]
	public SceneData scene;
	#endregion

	#region Methods
	public GameData ApplyOn(GameData data)
	{
		data.man.throwCooldown += man.throwCooldown;
		data.mallard.eatDuration += mallard.eatDuration;
		
		return data;
	}
	#endregion
}
