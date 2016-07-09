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
	UI.Text buyLabel;
	[SerializeField]
	UI.Text costLabel;
	[SerializeField]
	UI.Text researchLabel;

	public BoughtUpgradeHandler onPressedBuyButton;

	UpgradeManager manager;
	Upgrade currentUpgrade;
	GameData gameData;
	#endregion
	
	#region Properties
	#endregion

	#region Methods
	public void Setup(UpgradeManager manager)
	{
		this.manager = manager;
		buyButton.onClick.AddListener(OnBuyButtonSelected);

		researchLabel.gameObject.SetActive(false);
	}

	public void SetUpgrade(Upgrade upgrade)
	{
		currentUpgrade = upgrade;

		titleLabel.text = upgrade.upgradeName;
		descriptionLabel.text = upgrade.upgradeDescription;
		costLabel.text = string.Format("{0}", upgrade.cost);
	}

	public void DoUpdate()
	{
		if(currentUpgrade != null)
		{
			if(manager.IsResearching(currentUpgrade))
			{
				//researchLabel.gameObject.SetActive(true);
				//buyButton.gameObject.SetActive(false);
				//costLabel.gameObject.SetActive(false);
				researchLabel.text = "Researching...";
			}
			else if(manager.IsResearched(currentUpgrade))
			{
				//researchLabel.gameObject.SetActive(true);
				//buyButton.gameObject.SetActive(false);
				//costLabel.gameObject.SetActive(false);
				buyLabel.text = "Max";
			}
			else
			{
				//researchLabel.gameObject.SetActive(false);
				//buyButton.gameObject.SetActive(true);
				//costLabel.gameObject.SetActive(true);
				buyLabel.text = "Buy";

				bool buyable = currentUpgrade.cost <= gameData.points &&
					manager.Researchable(currentUpgrade); ;
				buyButton.interactable = buyable;

				var costColor = buyable ? Color.white : Color.red;
				buyLabel.color = costColor;
			}
		}
	}

	public void OnGameDataChanged(GameData gameData)
	{
		this.gameData = gameData;
	}

	void OnBuyButtonSelected()
	{
		if(onPressedBuyButton != null && currentUpgrade != null)
		{
			onPressedBuyButton(currentUpgrade);
		}
	}

	
	#endregion
}
