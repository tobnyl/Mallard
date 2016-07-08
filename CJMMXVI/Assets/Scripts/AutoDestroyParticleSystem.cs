using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AutoDestroyParticleSystem : MonoBehaviour
{
	#region Fields

	private ParticleSystem _particleSystem;

	#endregion

	#region Properties	
	#endregion

	#region Methods
	void OnEnable()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update()
	{
		if (!_particleSystem.IsAlive())
		{
			Destroy(gameObject);
		}
	}
	#endregion

}
