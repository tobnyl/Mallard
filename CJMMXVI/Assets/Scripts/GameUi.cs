using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
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
	[SerializeField]
	public RectTransform credits;

	Page _currentPage;
	List<GuiPage> pages;
	List<UI.Text> creditLabels;

	Coroutine creditsRoutine;
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
			page.gameObject.SetActive(true);
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

		creditLabels = new List<UI.Text>();
		credits.GetComponentsInChildren<UI.Text>(creditLabels);

		foreach(var label in creditLabels)
		{
			SetAlpha(label, 0.0f);
		}

		credits.gameObject.SetActive(false);
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

	public void OnUpgradeResearched(Upgrade upgrade)
	{
		for(int i = 0; i < pages.Count; ++i)
		{
			GuiPage page = pages[i];
			page.OnUpgradeResearched(upgrade);
		}
	}

	public void ShowCredits()
	{
		if(creditsRoutine != null)
		{
			return;
		}

		creditsRoutine = StartCoroutine(CreditsRoutine());
	}

	IEnumerator CreditsRoutine()
	{
		credits.gameObject.SetActive(true);

		const float fadeInDur = 2.0f;
		const float wait = 1.0f;
		const float finalWait = 5.0f;
		const float fadeOutDur = 3.0f;

		float timer = 0.0f;

		foreach(var label in creditLabels)
		{
			yield return new WaitForSeconds(wait);

			timer = fadeInDur;
			while(timer > 0.0f)
			{
				float t = Mathf.Clamp01(1.0f - (timer / fadeInDur));
				SetAlpha(label, t);
				timer -= Time.deltaTime;
				yield return null;
			}
			SetAlpha(label, 1.0f);
		}

		yield return new WaitForSeconds(finalWait);

		
		timer = fadeOutDur;
		while(timer > 0.0f)
		{
			float t = Mathf.Clamp01((timer / fadeOutDur));

			foreach(var label in creditLabels)
			{
				SetAlpha(label, t);	
			}
			timer -= Time.deltaTime;
			yield return null;
		}

		creditsRoutine = null;
	}

	void SetAlpha(UI.Text text, float alpha)
	{
		var c = text.color;
		c.a = alpha;
		text.color = c;
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
