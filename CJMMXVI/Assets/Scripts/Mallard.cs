using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Mallard : Entity
{
	#region Fields
	[SerializeField]
	public Food targetFood;
	[NonSerialized]
	public float eatTimer;
	#endregion
}
