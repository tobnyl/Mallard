using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class PhysicsLayers
{
	public static readonly PhysicsLayer cube = new PhysicsLayer("Cube");
	public static readonly PhysicsLayer player = new PhysicsLayer("Player");
}

public struct PhysicsLayer
{
	public readonly string name;
	public readonly int index;
	public readonly int mask;

	public PhysicsLayer(string name)
	{
		this.name = name;
		this.index = LayerMask.NameToLayer(name);
		this.mask = 1 << this.index;
	}
}