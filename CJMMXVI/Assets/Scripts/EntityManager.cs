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

	#region Fields
	// TODO: This differently
    [Serializable]
    private class Sounds
    {
        public Audio[] DuckSfx;
    }

	[SerializeField]
	Food foodPrefab;
	[SerializeField]
	Mallard mallardPrefab;
	[SerializeField]
	Feeder feederPrefab;

    [SerializeField]
    Sounds sfx;

	public event MallardEatHandler onMallardEat;

	Transform mallardSpawn;

	List<Entity> entities;

	List<Feeder> feeders;
	List<Mallard> mallards;
	Pool<Food> foodPool;
	List<Food> foods;

	GameData gameData;

	Transform objectsRoot;

	bool mouseWasDown;
	#endregion

	#region Methods
	public void Setup(Transform mallardSpawn)
	{
		var go = new GameObject("Entities");
		go.transform.parent = transform;
		objectsRoot = go.transform;

		feeders = new List<Feeder>(FindObjectsOfType<Feeder>());
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
			food.gameObject.SetActive(false);
		};
		foodPool.Grow(32);
		mallards = new List<Mallard>(FindObjectsOfType<Mallard>());
		foods = new List<Food>();

		//var feeder = Instantiate<Feeder>(feederPrefab);
		//AddEntity(feeder);
		//feeder.transform.position = feederSpawn.position;

		this.mallardSpawn = mallardSpawn;
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
	}

	public void AddEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Add((Feeder)entity); }
		if(entity is Mallard) { mallards.Add((Mallard)entity); }
		if(entity is Food) { foods.Add((Food)entity); }

		entity.transform.parent = objectsRoot;
	}

	public void RemoveEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Remove((Feeder)entity); }
		if(entity is Mallard) { mallards.Remove((Mallard)entity); }
		if(entity is Food) { foods.Remove((Food)entity); }
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

	void UpdateFood(Food food)
	{
		food.lifeTimer -= Time.deltaTime;
		if(food.lifeTimer <= 0.0f)
		{
			RemoveEntity(food);
			foodPool.Return(food);
		}
	}
	#endregion
}
