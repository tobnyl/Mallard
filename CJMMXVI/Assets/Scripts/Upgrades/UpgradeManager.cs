﻿using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Upgrade[] upgrades;

	// Upgrades waiting to be applied
	List<Upgrade> upgradeQueue;
	// Upgrades finalized and applied
	List<Upgrade> appliedUpgrades;

	// Upgrade being applied/researched/whatever
	Upgrade activeUpgrade;
	Coroutine activeUpgradeRoutine;
	#endregion
	
	#region Properties
	public Upgrade[] Upgrades
	{
		get { return upgrades; }
	}
	#endregion

	#region Methods
	public bool Researchable(Upgrade upgrade)
	{
		return upgradeQueue.Contains(upgrade) ||
			appliedUpgrades.Contains(upgrade) ||
			upgrade == activeUpgrade;
	}

	public void Research(Upgrade upgrade)
	{
		if(!Researchable(upgrade)) { return; }

		upgradeQueue.Add(upgrade);
	}

	void Update()
	{
		if(activeUpgrade == null && upgradeQueue.Count > 0)
		{
			Upgrade upgrade = upgradeQueue[0];
			upgradeQueue.RemoveAt(0);
			StartApplyingUpgrade(upgrade);
		}
	}

	void StartApplyingUpgrade(Upgrade upgrade)
	{
		activeUpgrade = upgrade;
		activeUpgradeRoutine = StartCoroutine(ApplyRoutine(upgrade));
	}

	IEnumerator ApplyRoutine(Upgrade upgrade)
	{
		// TODO: Do whatever here

		yield return new WaitForSeconds(upgrade.researchTime);

		// TODO: Some fun animation or something

		activeUpgradeRoutine = null;

		ApplyUpgrade(upgrade);
	}

	void ApplyUpgrade(Upgrade upgrade)
	{
		// TODO: Actual sideeffects, event?
	}
	#endregion
}
