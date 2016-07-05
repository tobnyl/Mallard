using UnityEngine;
using UE = UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(
	typeof(UpgradeManager),
	typeof(Map)
)]
public class Game : MonoBehaviour
{
	#region Fields
	[SerializeField]
	GameData initialGameData;

	[SerializeField]
	Upgrade[] upgrades;

	[Header("Read-only")]
	[SerializeField]
	GameData currentGameData;

	bool loaded;
	Coroutine loadRoutine;

	Map map;
	UpgradeManager upgradeMan;
	GameUi gui;
	#endregion
	
	#region Properties
	#endregion

	#region Methods
	#region Setup / Shutdown
	void OnEnable()
	{
		loadRoutine = StartCoroutine(Load());
	}

	IEnumerator Load()
	{
		currentGameData = initialGameData;

		gui = FindObjectOfType<GameUi>();
		if(gui == null)
		{
			SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
			// Need to wait a frame to find GameUi
			yield return null;
			gui = FindObjectOfType<GameUi>();
		}

		if(gui == null) { Debug.LogError("GUI could not be loaded. Add the GUI scene to build settings", this); }

		map = GetComponent<Map>();
		map.Setup();

		upgradeMan = GetComponent<UpgradeManager>();
		upgradeMan.Setup(upgrades);

		upgradeMan.onUpgradeFinished -= OnUpgradeFinished;
		upgradeMan.onUpgradeFinished += OnUpgradeFinished;

		loaded = true;
	}

	void OnDisable()
	{
		loaded = false;

		if(loadRoutine != null)
		{
			StopCoroutine(loadRoutine);
			loadRoutine = null;
		}
	}
	#endregion

	#region Updating
	void Update()
	{
		if(!loaded) { return; }

		upgradeMan.DoUpdate();

		// Temp
		gui.main.points.SetPoints(currentGameData.points);
	}
	#endregion

	#region Upgrades
	void OnUpgradeFinished(Upgrade upgrade)
	{
		currentGameData = upgrade.ApplyOn(currentGameData);
	}
	#endregion
	#endregion
}
