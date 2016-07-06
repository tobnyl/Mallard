using UnityEngine;
using UE = UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(
	typeof(UpgradeManager),
	typeof(EntityManager)
)]
public class Game : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Transform mallardSpawnPoint;
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
	EntityManager entityMan;
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

		map = GetComponentInChildren<Map>();

		if(map == null) { Debug.LogError("No Map attached to Game"); enabled = false; }
		else { map.Setup(); }

		entityMan = GetComponent<EntityManager>();

		if(entityMan == null) { Debug.LogError("No EntityManager attached to Game");}
		else
		{
			entityMan.Setup(mallardSpawnPoint);
			entityMan.onMallardEat -= OnMallardEat;
			entityMan.onMallardEat += OnMallardEat;
		}

		upgradeMan = GetComponent<UpgradeManager>();
		if(upgradeMan == null) { Debug.LogError("No UpgradeManager attached to Game"); }
		else
		{
			upgradeMan.Setup(upgrades);
			upgradeMan.onUpgradeFinished -= OnUpgradeFinished;
			upgradeMan.onUpgradeFinished += OnUpgradeFinished;
		}

		gui = FindObjectOfType<GameUi>();
		if(gui == null)
		{
			SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
			// Need to wait a frame to find GameUi
			yield return null;
			gui = FindObjectOfType<GameUi>();
		}

		if(gui == null) { Debug.LogError("GUI could not be loaded. Add the GUI scene to build settings", this); }

		gui.Setup(upgradeMan);

		OnGameDataChanged();

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

		entityMan.DoUpdate();
		upgradeMan.DoUpdate();

		// Temp
		
		gui.main.points.SetPoints(currentGameData.points);

		gui.DoUpdate();
	}
	#endregion

	#region Upgrades
	void OnUpgradeFinished(Upgrade upgrade)
	{
		currentGameData = upgrade.ApplyOn(currentGameData);
		OnGameDataChanged();
	}

	void OnMallardEat(Mallard mallard)
	{
		++currentGameData.points;
		OnGameDataChanged();
	}

	void OnGameDataChanged()
	{
		entityMan.OnGameDataChanged(currentGameData);
		gui.OnGameDataChanged(currentGameData);
	}
	#endregion
	#endregion
}
