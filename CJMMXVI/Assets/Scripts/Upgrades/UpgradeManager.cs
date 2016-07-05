using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
	#region Types
	public delegate void UpgradeFinishedHandler(Upgrade upgrade);
	#endregion

	#region Fields
	public event UpgradeFinishedHandler onUpgradeFinished;

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
	#region Interface
	public void Setup(Upgrade[] upgrades)
	{
		this.upgrades = upgrades;
		upgradeQueue = new List<Upgrade>();
		appliedUpgrades = new List<Upgrade>();

		activeUpgrade = null;
		activeUpgradeRoutine = null;
	}

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
	#endregion

	public void DoUpdate()
	{
		if(activeUpgrade == null && upgradeQueue.Count > 0)
		{
			StartNextUpgrade();
		}
	}

	void StartNextUpgrade()
	{
		Upgrade upgrade = upgradeQueue[0];
		upgradeQueue.RemoveAt(0);
		activeUpgrade = upgrade;
		activeUpgradeRoutine = StartCoroutine(ApplyRoutine(upgrade));
	}

	IEnumerator ApplyRoutine(Upgrade upgrade)
	{
		// TODO: Do whatever here

		yield return new WaitForSeconds(upgrade.researchTime);

		// TODO: Some fun animation or something

		activeUpgradeRoutine = null;

		FinalizeApplyingUpgrade(upgrade);
	}

	void FinalizeApplyingUpgrade(Upgrade upgrade)
	{
		activeUpgradeRoutine = null;
		activeUpgrade = null;

		appliedUpgrades.Add(upgrade);

		ApplyUpgrade(upgrade);
	}

	// Actual callback
	void ApplyUpgrade(Upgrade upgrade)
	{
		if(onUpgradeFinished != null) { onUpgradeFinished(upgrade); }
	}
	#endregion
}
