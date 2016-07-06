using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
	#region Types
	[Serializable]
	public class ManData
	{
		[SerializeField]
		public int breadThrown;
		[SerializeField]
		public float throwCooldown;
	}

	[Serializable]
	public class MallardData
	{
		[SerializeField]
		public float eatDuration;
		[SerializeField]
		public int count;
	}

	[Serializable]
	public class EnvData
	{
	}

	//[Serializable]
	//public class SceneData
	//{
	//	[SerializeField]
	//	public Transform[] toEnable;
	//	[SerializeField]
	//	public Transform[] toDisable;
	//}
	#endregion

	#region Fields
	[SerializeField]
	public string upgradeName;

	[SerializeField]
	[Multiline]
	public string upgradeDescription;

	[SerializeField]
	public int cost;

	[SerializeField]
	public float researchTime;

	[SerializeField]
	public Upgrade[] dependencies;

	[SerializeField]
	public ManData man;

	[SerializeField]
	public MallardData mallard;

	[SerializeField]
	public EnvData environment;

	//[SerializeField]
	//public SceneData scene;
	#endregion

	#region Methods
	public GameData ApplyOn(GameData data)
	{
		data.points -= cost;
		data.man.breadThrown += data.man.breadThrown;
		data.man.throwCooldown += man.throwCooldown;
		data.mallard.eatDuration += mallard.eatDuration;
		data.mallard.count += mallard.count;
		
		return data;
	}
	#endregion
}
