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
	}

	public override void OnUpgradeResearched(Upgrade upgrade)
	{
		upgradesList.OnUpgradeResearched(upgrade);
	}

	void OnBuyButtonSelected(Upgrade upgrade)
	{
		manager.Research(upgrade);
	}
	#endregion
}
