﻿using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityManager : MonoBehaviour
{
	#region Types
	public delegate void MallardEatHandler(Mallard mallard);
	#endregion

	#region Static Fields
	static readonly int THROW_MULT_HASH = Animator.StringToHash("ThrowMultiplier");

	static EntityManager instance;
	static readonly List<Entity> entitiesToRegister = new List<Entity>();
	#endregion

	#region Fields
	// TODO: This differently
    [Serializable]
    private class Sounds
    {
	    public Audio[] OldManSfx;
        public Audio[] DuckSfx;
    }
	
	[SerializeField]
	Food foodPrefab;
	[SerializeField]
	Mallard mallardPrefab;
	[SerializeField]
	Feeder feederPrefab;
	[SerializeField]
	GameObject WaterRingsPrefab;
	[SerializeField]
	GameObject WaterSplashPrefab;

    [SerializeField]
    Sounds sfx;

	[SerializeField]
	AnimationCurve breadCurve = AnimationCurve.Linear(
		timeStart: 0.0f,
		valueStart: 0.0f,
		timeEnd: 0.0f,
		valueEnd: 0.0f
	);

	public event MallardEatHandler onMallardEat;

	Transform mallardSpawn;

	Dictionary<int, Entity> idToEntity;
	List<Entity> entities;

	List<Feeder> feeders;
	List<Mallard> mallards;
	Pool<Food> foodPool;
	List<Food> foods;

	GameData gameData;

	Transform objectsRoot;

	bool mouseWasDown;

	int entityIdCounter;
	#endregion

	#region Methods
	public static void RegisterEntity(Entity entity)
	{
		if(instance == null)
		{
			entitiesToRegister.Add(entity);
		}
		else
		{
			instance.AddEntity(entity);
		}
	}

	public static void UnregisterEntity(Entity entity)
	{
		if(instance == null)
		{
			entitiesToRegister.Remove(entity);
		}
		else
		{
			instance.RemoveEntity(entity);
		}
	}

	public void Setup(Transform mallardSpawn)
	{
		instance = this;

		var go = new GameObject("Entities");
		go.transform.parent = transform;
		objectsRoot = go.transform;

		// new List<Feeder>(FindObjectsOfType<Feeder>());

		//for(int i = 0; i < feedersToRegister.Count; ++i)
		//{
		//	Feeder preRegistered = feedersToRegister[i];
		//	if(!feeders.Contains(preRegistered))
		//	{
		//		feeders.Add(preRegistered);
		//	}
		//}

		idToEntity = new Dictionary<int, Entity>();
		feeders = new List<Feeder>();
		mallards = new List<Mallard>(FindObjectsOfType<Mallard>());
		foods = new List<Food>();

		foodPool = new Pool<Food>(() =>
		{
			var obj = Instantiate(foodPrefab);
			obj.transform.parent = objectsRoot;
			return obj;
		});
		foodPool.onGet = (Food food) =>
		{
			food.gameObject.SetActive(true);
		};
		foodPool.onFree = (Food food) =>
		{
			food.Reset();
			food.gameObject.SetActive(false);
		};
		foodPool.Grow(32);

		//var feeder = Instantiate<Feeder>(feederPrefab);
		//AddEntity(feeder);
		//feeder.transform.position = feederSpawn.position;

		this.mallardSpawn = mallardSpawn;

		for(int i = 0; i < entitiesToRegister.Count; ++i)
		{
			Entity preReg = entitiesToRegister[i];
			AddEntity(preReg);
		}
		entitiesToRegister.Clear();
	}

	public void OnGameDataChanged(GameData gameData)
	{
		this.gameData = gameData;

		while(mallards.Count < gameData.mallard.count)
		{
			var mallard = Instantiate<Mallard>(mallardPrefab);
			AddEntity(mallard);
			Vector2 rand = UE.Random.insideUnitCircle;
			mallard.defaultPosition = mallardSpawn.position + new Vector3(rand.x, 0.0f, rand.y) * 3.0f;
			mallard.transform.position = mallard.defaultPosition;
			rand = UE.Random.insideUnitCircle.normalized;
			float angle = rand.ToAngle();
			mallard.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
		}

		for(int i = 0; i < feeders.Count; ++i)
		{
			Feeder feeder = feeders[i];
			if(feeder.animator == null) { continue; }

			GameData.FeederData data = FeederDataForKind(feeder.kind);

			if(feeder.animator.gameObject.activeInHierarchy)
			{
				feeder.animator.SetFloat(THROW_MULT_HASH, data.throwAnimSpeed);
			}
			if(feeder.additionalAnimator != null)
			{
				if(feeder.additionalAnimator.gameObject.activeInHierarchy)
				{
					feeder.additionalAnimator.SetFloat(THROW_MULT_HASH, data.throwAnimSpeed);
				}
			}
		}
	}

	public void AddEntity(Entity entity)
	{
		if(entity is Feeder)
		{
			feeders.Add((Feeder)entity);
			((Feeder)entity).ammo = FeederDataForKind(((Feeder)entity).kind).ammo;
		}
		if(entity is Mallard) { mallards.Add((Mallard)entity); }
		if(entity is Food) { foods.Add((Food)entity); }

		if(entity.id == 0) { entity.id = ++entityIdCounter; }

		idToEntity[entity.id] = entity;

		entity.transform.parent = objectsRoot;
	}

	public void RemoveEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Remove((Feeder)entity); }
		if(entity is Mallard) { mallards.Remove((Mallard)entity); }
		if(entity is Food) { foods.Remove((Food)entity); }

		idToEntity.Remove(entity.id);
	}
	
	public void DoUpdate(bool updateInput)
	{
		Feeder clickedFeeder = null;

		if(updateInput && Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			bool hit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, PhysicsLayers.player.mask);

			if(hit)
			{
				clickedFeeder = hitInfo.collider.GetComponentInChildren<Feeder>();
			}
		}

		for(int i = feeders.Count - 1; i >= 0; --i)
		{
			Feeder feeder = feeders[i];
			UpdateFeeder(
				feeder,
				wasSelected: clickedFeeder == feeder
			);
		}

		for(int i = mallards.Count - 1; i >= 0; --i)
		{
			Mallard mallard = mallards[i];
			UpdateMallard(mallard);
		}

		for(int i = foods.Count - 1; i >= 0; --i)
		{
			Food food = foods[i];
			UpdateFood(food);
		}
	}

	private void PlayRandomAudioFromList(Audio[] audioList)
	{
		if (audioList.Length > 0)
		{
			var index = UE.Random.Range(0, audioList.Length);

			AudioManager.Instance.Play(audioList[index], Vector3.zero);
		}
	}

	#endregion
}
