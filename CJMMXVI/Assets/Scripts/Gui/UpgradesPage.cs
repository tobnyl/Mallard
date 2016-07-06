using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradesPage : GuiPage
{
	#region Fields
	[Header("Info")]
	[SerializeField]
	UI.Text titleLabel;
	[SerializeField]
	UI.Text descriptionLabel;
	[SerializeField]
	UI.Button buyButton;

	[Header("Upgrades")]
	[SerializeField]
	float heightPerItem = 50.0f;
	[SerializeField]
	UpgradeButton upgradeBtnPrefab;
	[SerializeField]
	RectTransform upgradesRoot;

	UpgradeManager manager;
	List<UpgradeButton> upgradeButtons;
	#endregion

	#region Methods
	public void Setup(UpgradeManager upgradeManager)
	{
		manager = upgradeManager;
		upgradeButtons = new List<UpgradeButton>();

		Upgrade[] upgrades = manager.Upgrades;
		for(int i = 0; i < upgrades.Length; ++i)
		{
			Upgrade upgrade = upgrades[i];
			AddUpgradeButton(upgrade);
		}
	}

	void AddUpgradeButton(Upgrade upgrade)
	{
		var btn = Instantiate(upgradeBtnPrefab);
		btn.upgrade = upgrade;
		btn.SetTitle(upgrade.upgradeName);

		btn.button.onClick.AddListener(() =>
		{
			OnButtonSelected(btn);
		});

		upgradeButtons.Add(btn);

		var btnTrans = (RectTransform)btn.transform;
		btnTrans.SetParent(upgradesRoot, worldPositionStays: false);

		Vector2 min = upgradesRoot.offsetMin;
		Vector2 max = upgradesRoot.offsetMax;
		float fullHeight = manager.Upgrades.Length * heightPerItem;
		min.y = -fullHeight / 2.0f;
		max.y = fullHeight / 2.0f;
		upgradesRoot.offsetMin = min;
		upgradesRoot.offsetMax = max;
	}

	void OnButtonSelected(UpgradeButton button)
	{
		Debug.Log("Selected " + button.upgrade.upgradeName);
	}
	#endregion
}
