using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradesPage : GuiPage
{
	#region Fields
	[SerializeField]
	public UI.Button exitButton;
	[SerializeField]
	UpgradeInfoGui upgradeInfo;
	[SerializeField]
	UpgradesListGui upgradesList;

	List<UpgradeNodeGui> upgradeNodes;
	UpgradeManager manager;
	#endregion

	#region Methods
	public void Setup(UpgradeManager upgradeManager)
	{
		manager = upgradeManager;

		upgradesList.Setup(manager);
		upgradesList.onUpgradeSelected += OnUpgradeSelected;

		upgradeInfo.Setup(manager);
		upgradeInfo.onPressedBuyButton += OnBuyButtonSelected;
		upgradeInfo.SetUpgrade(manager.Upgrades[0]);

		upgradeNodes = new List<UpgradeNodeGui>();
		GetComponentsInChildren<UpgradeNodeGui>(upgradeNodes);
		for(int i = 0; i < upgradeNodes.Count; ++i)
		{
			UpgradeNodeGui node = upgradeNodes[i];
			node.Setup(manager);
			node.onSelection += OnUpgradeSelected;
		}
	}

	public void DoUpdate()
	{
		upgradeInfo.DoUpdate();
	}

	public void OnUpgradeSelected(Upgrade upgrade)
	{
		upgradeInfo.SetUpgrade(upgrade);
	}

	public override void OnGameDataChanged(GameData gameData)
	{
		upgradeInfo.OnGameDataChanged(gameData);

		for(int i = 0; i < upgradeNodes.Count; ++i)
		{
			UpgradeNodeGui node = upgradeNodes[i];
			node.OnGameDataChanged(gameData);
		}
	}

	public override void OnUpgradeResearched(Upgrade upgrade)
	{
		upgradesList.OnUpgradeResearched(upgrade);

		for(int i = 0; i < upgradeNodes.Count; ++i)
		{
			UpgradeNodeGui node = upgradeNodes[i];
			node.OnUpgradeResearched(upgrade);
		}
	}

	void OnBuyButtonSelected(Upgrade upgrade)
	{
		manager.Research(upgrade);
	}
	#endregion
}
