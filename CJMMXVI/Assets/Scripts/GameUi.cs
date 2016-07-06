using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameUi : MonoBehaviour
{
	#region Types
	public enum Page
	{
		Main,
		Upgrades,
		//Pause,
		//MainMenu
	}
	#endregion

	#region Fields
	[SerializeField]
	public MainPage main;
	[SerializeField]
	public UpgradesPage upgrades;

	Page _currentPage;
	List<GuiPage> pages;
	#endregion
	
	#region Properties
	public Page currentPage
	{
		get { return _currentPage;}
		set
		{
			_currentPage = value;
			HideAllPages();
			GuiPage page = GetPageGui(_currentPage);
			page.Hidden = false;
		}
	}
	#endregion

	#region Methods
	public void Setup(UpgradeManager upgradeMan)
	{
		pages = new List<GuiPage>();

		pages.Add(main);
		pages.Add(upgrades);

		upgrades.Setup(upgradeMan);

		for(int i = 0; i < pages.Count; ++i)
		{
			GuiPage page = pages[i];
			page.gameObject.active = true;
		}

		currentPage = Page.Main;

		main.upgradesButton.onClick.AddListener(() =>
		{
			currentPage = Page.Upgrades;
		});

		upgrades.exitButton.onClick.AddListener(() =>
		{
			currentPage = Page.Main;
		});
	}

	public void DoUpdate()
	{
		upgrades.DoUpdate();
	}

	public void OnGameDataChanged(GameData gameData)
	{
		for(int i = 0; i < pages.Count; ++i)
		{
			GuiPage page = pages[i];
			page.OnGameDataChanged(gameData);
		}
	}

	void HideAllPages()
	{
		for(int i = 0; i < pages.Count; ++i)
		{
			GuiPage page = pages[i];
			page.Hidden = true;
		}
	}

	GuiPage GetPageGui(Page page)
	{
		switch(page)
		{
			case Page.Main: { return main; }
			case Page.Upgrades: { return upgrades; }
		}

		throw new Exception(string.Format("Unhandled page {0}", page));
	}
	#endregion
}
