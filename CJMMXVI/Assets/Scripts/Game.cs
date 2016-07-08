using UnityEngine;
using UE = UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(
	typeof(UpgradeManager),
	typeof(EntityManager)
)]
public class Game : MonoBehaviour
{
	#region Types
	[Serializable]
	struct FloatRange
	{
		public float min;
		public float max;

		public float Lerp(float t)
		{
			return Mathf.Lerp(min, max, t);
		}
	}

	[Serializable]
	class Visuals
	{
		[SerializeField]
		public ColorCorrectionCurves colorCurves;
		[SerializeField]
		public FloatRange saturationRange;
		[SerializeField]
		public VignetteAndChromaticAberration vignette;
		[SerializeField]
		public FloatRange vignetteRange;

		[SerializeField]
		public Camera[] cameras;
		[SerializeField]
		public FloatRange cameraZoomRange;
	}

	[Serializable]
	class Sounds
	{
		[Serializable]
		public class MusicTrack
		{
			[SerializeField]
			public Audio audio;
			[SerializeField]
			public float stopsAt;
		}

		[SerializeField]
		public MusicTrack[] tracks;
	}
	#endregion

	#region Fields
	[SerializeField]
	Visuals visuals;

	[SerializeField, Space(5.0f)]
	Transform mallardSpawnPoint;
	[SerializeField]
	Transform deactivateOnStart;
	[SerializeField]
	GameData initialGameData;

	[Header("Read-only")]
	[SerializeField]
	GameData currentGameData;

	[SerializeField, Range(0.0f, 1.0f)]
	float fuckedUpOMeter;

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
		fuckedUpOMeter = -1.0f;

		var toDeactivate = new List<Transform>();

		if(deactivateOnStart != null)
		{
			foreach(Transform child in deactivateOnStart)
			{
				toDeactivate.Add(child);
			}
		}

		sceneObjectsLookup = new Dictionary<string, GameObject>();
		currentGameData = initialGameData;
		currentGameData.Limit();

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

		foreach(Transform obj in toDeactivate)
		{
			obj.gameObject.SetActive(false);
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

		entityMan.DoUpdate(updateInput: gui.currentPage == GameUi.Page.Main);
		upgradeMan.DoUpdate();
		map.DoUpdate();

		// Temp
#if UNITY_EDITOR
		entityMan.OnGameDataChanged(currentGameData);
#endif
		gui.main.points.SetPoints(currentGameData.points);

		gui.DoUpdate();

		if(fuckedUpOMeter != currentGameData.fuckedUpOMeter)
		{
			fuckedUpOMeter = Mathf.Lerp(fuckedUpOMeter, currentGameData.fuckedUpOMeter, Time.deltaTime);

			if(Mathf.Abs(fuckedUpOMeter - currentGameData.fuckedUpOMeter) < 0.0001f)
			{
				fuckedUpOMeter = currentGameData.fuckedUpOMeter;
			}

			if(visuals.vignette != null)
			{
				visuals.vignette.intensity = visuals.vignetteRange.Lerp(fuckedUpOMeter);
			}

			if(visuals.colorCurves != null)
			{
				visuals.colorCurves.saturation = visuals.saturationRange.Lerp(fuckedUpOMeter);
			}

			for(int i = 0; i < visuals.cameras.Length; ++i)
			{
				Camera cam = visuals.cameras[i];
				if(cam != null)
				{
					cam.orthographicSize = visuals.cameraZoomRange.Lerp(fuckedUpOMeter);
				}
			}
		}
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

		if(upgrade.environment.enableRadioactiveWater)
		{
			map.EnableRadioactiveWater();
		}
	}

	void OnMallardEat(Mallard mallard)
	{
		currentGameData.points += Mathf.Max(1, currentGameData.mallard.pointsPerQuack);
		OnGameDataChanged();
	}

	void OnGameDataChanged()
	{
		currentGameData.Limit();

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
