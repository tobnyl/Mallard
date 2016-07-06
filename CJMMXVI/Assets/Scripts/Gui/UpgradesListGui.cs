using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradesListGui : MonoBehaviour
{
	#region Types
	public delegate void UpgradeSelectedHandler(Upgrade upgrade);
	#endregion

	#region Fields
	[SerializeField]
	float heightPerItem = 50.0f;
	[SerializeField]
	UpgradeButtonGui btnPrefab;
	[SerializeField]
	RectTransform contentRoot;

	public UpgradeSelectedHandler onUpgradeSelected;

	UpgradeManager manager;
	List<UpgradeButtonGui> upgradeButtons;
	#endregion

	#region Methods
	public void Setup(UpgradeManager manager)
	{
		this.manager = manager;
		upgradeButtons = new List<UpgradeButtonGui>();

		Upgrade[] upgrades = manager.Upgrades;
		for(int i = 0; i < upgrades.Length; ++i)
		{
			Upgrade upgrade = upgrades[i];
			AddUpgradeButton(upgrade);
		}
	}

	public void AddUpgradeButton(Upgrade upgrade)
	{
		var btn = Instantiate(btnPrefab);
		btn.upgrade = upgrade;
		btn.SetTitle(upgrade.upgradeName);

		btn.button.onClick.AddListener(() =>
		{
			OnButtonSelected(btn);
		});

		upgradeButtons.Add(btn);

		var btnTrans = (RectTransform)btn.transform;
		btnTrans.SetParent(contentRoot, worldPositionStays: false);

		Vector2 min = contentRoot.offsetMin;
		Vector2 max = contentRoot.offsetMax;
		float fullHeight = manager.Upgrades.Length * heightPerItem;
		min.y = -fullHeight / 2.0f;
		max.y = fullHeight / 2.0f;
		contentRoot.offsetMin = min;
		contentRoot.offsetMax = max;
	}

	void OnButtonSelected(UpgradeButtonGui button)
	{
		Debug.Log("Selected " + button.upgrade.upgradeName);
		if(onUpgradeSelected != null)
		{
			onUpgradeSelected(button.upgrade);
		}
	}
	#endregion
}
