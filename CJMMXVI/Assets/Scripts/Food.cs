using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Food : Entity
{
	#region Fields
	[SerializeField]
	float decayDuration;

	[NonSerialized]
	public float lifeTimer;
	#endregion

	#region Properties
	public bool isDead
	{
		get { return lifeTimer > 0.0f; }
	}
	#endregion

	#region Methods
	public void DoUpdate()
	{
		if(lifeTimer > 0.0f)
		{
			lifeTimer -= Time.deltaTime;
		}
		else
		{
			isDead = true;
		}
	}
	#endregion
}
