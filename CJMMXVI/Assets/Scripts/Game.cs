﻿using UnityEngine;
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
	Transform deactivateOnStart;
	[SerializeField]
	GameData initialGameData;

	[Header("Read-only")]
	[SerializeField]
	GameData currentGameData;

	Upgrade[] upgrades;

	bool loaded;
	Coroutine loadRoutine;

	Map map;
	UpgradeManager upgradeMan;
	EntityManager entityMan;
	GameUi gui;

	Dictionary<string, GameObject> sceneObjectsLookup;
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
		sceneObjectsLookup = new Dictionary<string, GameObject>();
		currentGameData = initialGameData;

		this.upgrades = Resources.LoadAll<Upgrade>("Upgrades");

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

		foreach(Upgrade upgrade in upgradeMan.Upgrades)
		{
			CacheSceneObjects(upgrade.environment.toActivate);
			CacheSceneObjects(upgrade.environment.toDeactivate);
		}

		if(deactivateOnStart != null)
		{
			foreach(Transform child in deactivateOnStart)
			{
				child.gameObject.SetActive(false);
			}
		}

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
#if UNITY_EDITOR
		entityMan.OnGameDataChanged(currentGameData);
#endif
		gui.main.points.SetPoints(currentGameData.points);

		gui.DoUpdate();
	}
	#endregion

	#region Upgrades
	void OnUpgradeFinished(Upgrade upgrade)
	{
		Upgrade.SceneObject[] toActive = upgrade.environment.toActivate;
		SetSceneObjectsActive(toActive, true);
		Upgrade.SceneObject[] toInactive = upgrade.environment.toDeactivate;
		SetSceneObjectsActive(toInactive, false);

		currentGameData = upgrade.ApplyOn(currentGameData);
		OnGameDataChanged();

		gui.OnUpgradeResearched(upgrade);

		AudioManager.Instance.Play(upgrade.researchSound, Vector3.zero);
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

	void CacheSceneObjects(Upgrade.SceneObject[] objs)
	{
		for(int i = 0; i < objs.Length; ++i)
		{
			Upgrade.SceneObject obj = objs[i];
			GameObject go = GameObject.Find(obj.scenePath);
			if(go == null)
			{
				Debug.LogError("Unable to find object at scene path \"" + obj.scenePath + "\"");
				continue;
			}
			sceneObjectsLookup[obj.scenePath] = go;
		}
	}

	void SetSceneObjectsActive(Upgrade.SceneObject[] objs, bool active)
	{
		for(int i = 0; i < objs.Length; ++i)
		{
			Upgrade.SceneObject obj = objs[i];
			if(!sceneObjectsLookup.ContainsKey(obj.scenePath))
			{
				continue;
			}

			GameObject go = sceneObjectsLookup[obj.scenePath];
			go.SetActive(active);
		}
	}
	#endregion
	#endregion
}
