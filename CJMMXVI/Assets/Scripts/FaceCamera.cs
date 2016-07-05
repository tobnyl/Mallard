using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FaceCamera : MonoBehaviour
{
	#region Fields
	#endregion
	
	#region Properties	
	#endregion

	#region Methods
	void OnEnable()
	{
	}
	
	void LateUpdate()
	{
		var camPos = Camera.main.transform.position;
		Vector3 selfToCamPos = camPos - transform.position;
		var lookRot = Quaternion.LookRotation(selfToCamPos.normalized);
		transform.rotation = lookRot;
	}
	#endregion
}
