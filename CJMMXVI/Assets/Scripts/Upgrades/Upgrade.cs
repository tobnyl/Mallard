﻿using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
	#region Types
	[Serializable]
	public class FeederData
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
		[SerializeField]
		public float speed;
		[SerializeField]
		public float acceleration;
		[SerializeField]
		public float rotationSpeed;
	}

	[Serializable]
	public class SceneObject
	{
		[SerializeField]
		public string scenePath;
	}

	[Serializable]
	public class EnvData
	{
		[SerializeField]
		public SceneObject[] toActivate;
		[SerializeField]
		public SceneObject[] toDeactivate;
	}
	#endregion

	#region Fields
	[SerializeField]
	public Sprite icon;

	[SerializeField]
	public string upgradeName;

	[SerializeField]
	[Multiline]
	public string upgradeDescription;

	[SerializeField]
	public Audio researchSound;

	[SerializeField]
	public int cost;

	[SerializeField]
	public float researchTime;

	[SerializeField]
	public Upgrade[] dependencies;

	[SerializeField]
	public FeederData man;

	[SerializeField]
	public MallardData mallard;

	[SerializeField]
	public EnvData environment;

	[SerializeField]
	public FeederData autoFeeders;
	#endregion

	#region Methods
	public GameData ApplyOn(GameData data)
	{
		data.points -= cost;
		
		data.man = ApplyFeederData(data.man, man);
		data.autoFeeders = ApplyFeederData(data.autoFeeders, autoFeeders);

		data.mallard.eatDuration += mallard.eatDuration;
		data.mallard.count += mallard.count;
		data.mallard.speed += mallard.speed;
		
		data.mallard.acceleration += mallard.acceleration;
		data.mallard.rotationSpeed += mallard.rotationSpeed;
		
		return data;
	}

	GameData.FeederData ApplyFeederData(GameData.FeederData gameData, FeederData upgr)
	{
		gameData.breadThrown += upgr.breadThrown;
		gameData.throwCooldown += upgr.throwCooldown;
		
		return gameData;
	}
	#endregion
}
