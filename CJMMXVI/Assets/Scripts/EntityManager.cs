using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EntityManager : MonoBehaviour
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
	List<Food> foods;

	GameData gameData;
	#endregion

	#region Methods
	public void Setup(Transform mallardSpawn)
	{
		feeders = new List<Feeder>(FindObjectsOfType<Feeder>());
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
	}

	public void RemoveEntity(Entity entity)
	{
		if(entity is Feeder) { feeders.Remove((Feeder)entity); }
		if(entity is Mallard) { mallards.Remove((Mallard)entity); }
	    if (entity is Food)
	    {
	        var duckSfxIndex = UE.Random.Range(0, sfx.DuckSfx.Length);
                        
	        AudioManager.Instance.Play(sfx.DuckSfx[duckSfxIndex], Vector3.zero);
	        foods.Remove((Food)entity);
	    }

		Destroy(entity.gameObject);
	}
	
	public void DoUpdate()
	{
		Feeder clickedFeeder = null;

		if(Input.GetMouseButtonDown(0))
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

	void UpdateFeeder(Feeder feeder, bool wasSelected)
	{
		if(feeder.feedTimer > 0.0f)
		{
			feeder.feedTimer -= Time.deltaTime;
			return;
		}

		bool shouldFeed =
			feeder.autoFeed ||
			(feeder.playerControlled && wasSelected);

		if(!shouldFeed) { return; }

		int thrown = Mathf.Max(1, gameData.man.breadThrown);
		for(int i = 0; i < thrown; ++i)
		{
			var food = Instantiate<Food>(foodPrefab);
			AddEntity(food);
			food.transform.position =
				mallardSpawn.transform.position +
				Vector3.right * UE.Random.Range(-2.0f, 2.0f) +
				Vector3.forward * UE.Random.Range(0.0f, 4.0f);
			food.lifeTimer = food.decayDuration;
		}

		feeder.feedTimer = gameData.man.throwCooldown;
	}

	void UpdateMallard(Mallard mallard)
	{
		if(mallard.eatTimer > 0.0f)
		{
			mallard.eatTimer -= Time.deltaTime;

			if(mallard.eatTimer <= 0.0f)
			{
				if(onMallardEat != null) { onMallardEat(mallard); }

				Debug.Log("Quack");
			}

			return;
		}

		Transform mallardTrans = mallard.transform;
		Vector3 mallardPos = mallardTrans.position;

		if(mallard.targetFood == null)
		{
			Food closestFood = null;
			float closestDist = Mathf.Infinity;

			for(int i = 0; i < foods.Count; ++i)
			{
				Food food = foods[i];

				float distSqr = (food.transform.position - mallardPos).sqrMagnitude;
				if(distSqr < closestDist)
				{
					closestFood = food;
					closestDist = distSqr;
				}
			}

			mallard.targetFood = closestFood;
		}
		else if(!foods.Contains(mallard.targetFood))
		{
			// Not sure why this is happening
			mallard.targetFood = null;
		}

		Vector3 targetPos = mallard.targetFood == null ?
			mallard.defaultPosition :
			mallard.targetFood.transform.position;
		
		Vector3 fromMallardToFood = targetPos - mallardPos;
		Vector3 dir = fromMallardToFood.normalized;

		float angle = Mathf.Atan2(dir.x, -dir.y);
		mallard.transform.rotation = Quaternion.Euler(0.0f, dir.ToAngleXZ(), 0.0f);
		//mallard.transform.rotation = Quaternion.Euler(0.0f, angle * Mathf.Rad2Deg, 0.0f);

		Vector3 vel = dir * gameData.mallard.speed;
		vel = Vector3.ClampMagnitude(vel, fromMallardToFood.magnitude);
		mallardTrans.position += vel;

		float finalDist = Vector3.Distance(mallardTrans.position, targetPos);
		if(finalDist < 0.001f)
		{
			if(mallard.targetFood != null)
			{
				Debug.Log(mallardTrans.gameObject.name + " reached target food", this);
				// Reached food!
				RemoveEntity(mallard.targetFood);
				mallard.eatTimer = gameData.mallard.eatDuration;
				mallard.targetFood = null;
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
