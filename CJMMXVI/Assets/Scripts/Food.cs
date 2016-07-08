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

	private ParticleSystem _waterRings;

	#endregion

	#region Methods

	void OnEnable()
	{
		_waterRings = gameObject.GetComponentInChildren<ParticleSystem>();
		_waterRings.playOnAwake = false;
	}

	public void PlayWaterRings()
	{
		if (!_waterRings.IsAlive())
		{
			_waterRings.Play();
		}
	}

	public void Reset()
	{
		lifeTimer = 0.0f;
		originatedFrom = null;
	}

	#endregion
}
