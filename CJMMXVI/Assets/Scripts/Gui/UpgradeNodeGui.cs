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
	Transform levelRoot;
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

		Refresh();
	}

	public void OnGameDataChanged(GameData gameData)
	{
	}

	public void OnUpgradeResearched(Upgrade upgrade)
	{
		Refresh();
		if(-1 != Array.IndexOf(upgradeLevels, upgrade))
		{
			if(onSelection != null) { onSelection(activeLevel); }
		}
	}

	void Refresh()
	{
		iconButton.interactable = false;

		int level = 0;
		for(; level < upgradeLevels.Length; ++level)
		{
			activeLevel = upgradeLevels[level];

			bool researched = manager.IsResearched(activeLevel);
			if(!researched)
			{
				iconButton.interactable = manager.AllDependenciesResearched(activeLevel);
					//manager.Researchable(activeLevel);
				break;
			}
		}

		// Enable last level
		if(level == upgradeLevels.Length && manager.IsResearched(activeLevel))
		{
			iconButton.interactable = true;
		}

		if(upgradeLevels.Length <= 1)
		{
			levelRoot.gameObject.SetActive(false);
		}
		else
		{
			levelRoot.gameObject.SetActive(true);


			string no =
				level == 0 ? "-" :
				level == 1 ? "I" :
				level == 2 ? "II" :
				"III";

			levelLabel.text = no;
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
