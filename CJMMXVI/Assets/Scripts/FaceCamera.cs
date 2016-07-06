using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class FaceCamera : MonoBehaviour
{
	#region Types
	[Serializable]
	public class Direction
	{
		[SerializeField]
		public Sprite sprite;
		[SerializeField]
		public bool flipped;
	}

	[Serializable]
	public class Directions
	{
		[SerializeField]
		public Transform rotationTransform;

		[SerializeField]
		public Direction n;
		[SerializeField]
		public Direction ne;
		[SerializeField]
		public Direction e;
		[SerializeField]
		public Direction se;
		[SerializeField]
		public Direction s;
		[SerializeField]
		public Direction sw;
		[SerializeField]
		public Direction w;
		[SerializeField]
		public Direction nw; 
	}
	#endregion

	#region Fields
	[SerializeField]
	Directions dirs;
	[SerializeField]
	SpriteRenderer renderer;

	[SerializeField, ReadOnly]
	float rotation;
	[SerializeField, ReadOnly]
	int dirIndex;

	Transform cachedTrans;
	Transform camTrans;
	Direction lastDir;
	#endregion

	#region Methods
	void OnEnable()
	{
		cachedTrans = transform;
		camTrans = Camera.main.transform;

		if(cachedTrans == null || camTrans == null)
		{
			enabled = false;
		}
	}
	
	void LateUpdate()
	{
		if(dirs.rotationTransform != null)
		{
			float camFwd = camTrans.rotation.eulerAngles.y;
			rotation = dirs.rotationTransform.rotation.eulerAngles.y;
			rotation = camFwd - rotation;
			rotation = 360.0f - NormalizeAngle360(rotation);

			Direction dir = AngleToDirection(rotation, dirs);
			if(dir != lastDir)
			{
				lastDir = dir;
				renderer.sprite = dir.sprite;
				renderer.flipX = dir.flipped;
			}
		}

		Vector3 selfToCamPos = cachedTrans.position - camTrans.position;
		var lookRot = Quaternion.LookRotation(selfToCamPos.normalized);
		transform.rotation = lookRot;
	}

	float NormalizeAngle180(float angle)
	{
		angle = (angle + 180.0f) % 360.0f;
		if(angle < 0) { return angle + 360.0f; }
		return angle - 180.0f;
	}

	float NormalizeAngle360(float angle)
	{
		angle = angle % 360.0f;
		if(angle < 0.0f) { return angle + 360.0f; }
		return angle;
	}

	Direction AngleToDirection(float angle, Directions dirs)
	{
		const float degree = 360.0f / 8.0f;
		angle = angle + degree / 2.0f;
		angle = NormalizeAngle360(angle);

		if(angle >= 0 * degree && angle < 1 * degree)
			return dirs.n;
		if(angle >= 1 * degree && angle < 2 * degree)
			return dirs.ne;
		if(angle >= 2 * degree && angle < 3 * degree)
			return dirs.e;
		if(angle >= 3 * degree && angle < 4 * degree)
			return dirs.se;
		if(angle >= 4 * degree && angle < 5 * degree)
			return dirs.s;
		if(angle >= 5 * degree && angle < 6 * degree)
			return dirs.sw;
		if(angle >= 6 * degree && angle < 7 * degree)
			return dirs.w;
		if(angle >= 7 * degree && angle < 8 * degree)
			return dirs.nw;

		throw new Exception("Weird angle " + degree);
	}
	#endregion
}
