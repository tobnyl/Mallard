using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Extensions
{
	public static float ToAngle(this Vector2 v)
	{
		float a = Mathf.Atan2(v.x, v.y);
		if(v.x < 0.0f) { return 360.0f - (a * Mathf.Rad2Deg * -1.0f); }

		return v.x < 0.0f ?
			a * Mathf.Rad2Deg * -1.0f :
			a * Mathf.Rad2Deg;
	}

	public static float ToAngleXZ(this Vector3 v)
	{
		return new Vector2(v.x, v.z).ToAngle();
	}
}
