using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Feeder : Entity
{
	#region Fields
	[SerializeField]
	public bool playerControlled;
	[SerializeField]
	public bool autoFeed;
	[SerializeField]
	public Transform feedOrigin;

	[ReadOnly]
	public float feedTimer;
	#endregion
}
