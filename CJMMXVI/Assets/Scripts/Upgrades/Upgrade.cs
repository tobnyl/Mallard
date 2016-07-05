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
	}

	[Serializable]
	public class MallardData
	{
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
}
