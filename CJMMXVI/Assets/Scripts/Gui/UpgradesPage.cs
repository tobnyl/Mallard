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
	}

	public void OnUpgradeSelected(Upgrade upgrade)
	{

	}

	void OnBuyButtonSelected()
	{
	
	}
	#endregion
}
