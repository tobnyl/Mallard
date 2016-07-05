using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Food foodPrefab;

	List<Entity> entities;

	List<Feeder> feeders;
	List<Mallard> mallards;
	List<Food> foods;
	#endregion
	
	#region Properties	
	#endregion

	#region Methods
	public void Setup()
	{
		feeders = new List<Feeder>();
		mallards = new List<Mallard>();
		foods = new List<Food>();
	}

	public void AddEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Add((Feeder)entity); }
		if(entity is Mallard) { mallard.Add((Mallard)entity); }
		if(entity is Food) { foods.Add((Food)entity); }
	}

	public void RemoveEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Remove((Feeder)entity); }
		if(entity is Mallard) { mallard.Remove((Mallard)entity); }
		if(entity is Food) { foods.Remove((Food)entity); }
	}
	
	public void DoUpdate()
	{
		for(int i = 0; i < feeders.Count; ++i)
		{
			Feeder feeder = feeders[i];
			feeder.DoUpdate();
		}

		for(int i = mallards.Count; i < mallards.Count; --i)
		{
			Mallard mallard = mallards[i];
			UpdateMallard(mallard);
		}

		for(int i = foods.Count - 1; i >= 0; --i)
		{
			Food food = food[i];
			food.DoUpdate();
		}
	}

	void UpdateMallard(Mallard mallard)
	{
		if(mallard.targetFood == null)
		{
			for(int i = 0; i < foods.Count; ++i)
			{
				Food food = foods[i];
			}
		}
	}

	void UpdateFood(Food food)
	{
		food.lifeTimer -= Time.deltaTime;
		if(food.lifeTimer <= 0.0f)
		{
			RemoveEntity(food);
		}
	}
	#endregion
}
