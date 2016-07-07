using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Food : Entity
{
	#region Fields
	[SerializeField]
	public float decayDuration;

	[SerializeField, ReadOnly]
	public Vector3 startPos;
	[SerializeField, ReadOnly]
	public Vector3 targetPos;
	[SerializeField, ReadOnly]
	public float airTimeRemaining;
	[SerializeField, ReadOnly]
	public float airTimeDuration;

	[SerializeField, ReadOnly]
	public float lifeTimer;
	[SerializeField, ReadOnly]
	public Feeder originatedFrom;
	#endregion

	#region Methods
	public void Reset()
	{
		lifeTimer = 0.0f;
		originatedFrom = null;
	}
	#endregion
}
