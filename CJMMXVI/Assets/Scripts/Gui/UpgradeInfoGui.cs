using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradeInfoGui : MonoBehaviour
{
	#region Types
	public delegate void BoughtUpgradeHandler(Upgrade upgrade);
	#endregion

	#region Fields
	[SerializeField]
	UI.Text titleLabel;
	[SerializeField]
	UI.Text descriptionLabel;
	[SerializeField]
	UI.Button buyButton;
	[SerializeField]
	UI.Text costLabel;

	public BoughtUpgradeHandler onBoughtUpgrade;

	Upgrade currentUpgrade;
	#endregion
	
	#region Properties
	#endregion

	#region Methods
	public void SetUpgrade(Upgrade upgrade)
	{
		currentUpgrade = upgrade;

		titleLabel.text = upgrade.upgradeName;
		descriptionLabel.text = upgrade.upgradeDescription;
		costLabel.text = string.Format("{0} Q$", upgrade.cost);
	}

	void OnBuyButtonSelected()
	{
		if(onBoughtUpgrade != null && currentUpgrade != null)
		{
			onBoughtUpgrade(currentUpgrade);
		}
	}
	#endregion
}
