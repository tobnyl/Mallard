using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
	[SerializeField, ReadOnly]
	public int id;
}
