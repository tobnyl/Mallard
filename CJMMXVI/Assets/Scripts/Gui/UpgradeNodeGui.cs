using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradeNodeGui : MonoBehaviour
{
	#region Types
	public delegate void SelectedHandler(Upgrade upgrade);
	#endregion

	#region Fields
	[SerializeField]
	UI.Button iconButton;
	[SerializeField]
	UI.Image iconImage;
	[SerializeField]
	UI.Text levelLabel;

	[SerializeField]
	Upgrade[] upgradeLevels;

	public event SelectedHandler onSelection;

	Upgrade activeLevel;
	UpgradeManager manager;
	#endregion
	
	#region Properties
	#endregion

	#region Methods
	public void Setup(UpgradeManager manager)
	{
		this.manager = manager;
		iconButton.onClick.AddListener(() =>
		{
			if(activeLevel == null || onSelection == null) { return; }

			onSelection(activeLevel);
		});
	}

	public void OnGameDataChanged(GameData gameData)
	{
	}

	public void OnUpgradeResearched(Upgrade upgrade)
	{
		Refresh();
		if(onSelection != null) { onSelection(activeLevel); }
	}

	void Refresh()
	{
		int level = 0;
		for(; level < upgradeLevels.Length; ++level)
		{
			activeLevel = upgradeLevels[level];

			bool researched = manager.IsResearched(activeLevel);
			if(!researched)
			{
				iconButton.interactable = manager.Researchable(activeLevel);
				break;
			}
		}

		if(upgradeLevels.Length <= 1)
		{
			levelLabel.gameObject.SetActive(false);
		}
		else
		{
			levelLabel.gameObject.SetActive(true);
			levelLabel.text = level.ToString();
		}

		if(activeLevel != null)
		{
			if(activeLevel.icon != null)
			{
				iconImage.sprite = activeLevel.icon;
			}
		}
	}
	#endregion
}
