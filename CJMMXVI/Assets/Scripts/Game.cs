using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(
	typeof(UpgradeManager),
	typeof(Map)
)]
public class Game : MonoBehaviour
{
	#region Fields
	[SerializeField]
	GameData initialGameData;

	[SerializeField]
	Upgrade[] upgrades;

	[Header("Read-only")]
	[SerializeField]
	GameData currentGameData;

	Map map;
	UpgradeManager upgradeMan;
	#endregion
	
	#region Properties
	#endregion

	#region Methods
	void OnEnable()
	{
		currentGameData = initialGameData;

		map = GetComponent<Map>();
		map.Setup();

		upgradeMan = GetComponent<UpgradeManager>();
		upgradeMan.Setup(upgrades);
		upgradeMan.onUpgradeFinished += OnUpgradeFinished;
	}

	void OnDisable()
	{
		upgradeMan.onUpgradeFinished -= OnUpgradeFinished;
	}
	
	void Update()
	{
		upgradeMan.DoUpdate();
	}

	void OnUpgradeFinished(Upgrade upgrade)
	{
		currentGameData = upgrade.ApplyOn(currentGameData);
	}
	#endregion
}
