using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Mallard : Entity
{
	#region Fields
	[SerializeField, ReadOnly]
	public Vector3 defaultPosition;
	[SerializeField, ReadOnly]
	public Food targetFood;
	[SerializeField, ReadOnly]
	public float eatTimer;
	#endregion
}
