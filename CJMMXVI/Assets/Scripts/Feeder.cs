using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Feeder : Entity
{
	#region Types
	public enum Kind
	{
		Player,
		Npc,
		Manual,
		Auto,
	}
	#endregion

	#region Fields
	[SerializeField]
	public Kind kind;
	[SerializeField]
	public bool playerControlled;
	[SerializeField]
	public bool autoFeed;
	[SerializeField]
	public bool usesAmmo;
	[SerializeField, ReadOnly]
	public int ammo;
	[SerializeField]
	public Transform feedOrigin;
	[SerializeField]
	public Animator animator;
	[SerializeField]
	public float throwDelayDuration;

	[ReadOnly]
	public float feedTimer;
	[ReadOnly]
	public float throwDelayTimer;
	#endregion

	#region Methods
	void OnEnable()
	{
		EntityManager.RegisterEntity(this);
	}

	void OnDisable()
	{
		EntityManager.UnregisterEntity(this);
	}
	#endregion
}
