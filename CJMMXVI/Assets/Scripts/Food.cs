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
	public float lifeTimer;
	#endregion
}
