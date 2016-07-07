using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaterCube : MonoBehaviour
{
	#region Fields

	private MeshRenderer _meshRenderer;
	private ParticleSystem _surfacePs;
	private ParticleSystem _radioactivePs;

	#endregion
	
	#region Properties	
	#endregion

	#region Methods
	void OnEnable()
	{
		var ps = GetComponentsInChildren<ParticleSystem>();
		_radioactivePs = ps[0];
		_surfacePs = ps[1];

		_radioactivePs.enableEmission = false;

		_meshRenderer = GetComponent<MeshRenderer>();
	}

	public void SetMaterial(Material material)
	{
		_meshRenderer.material = material;
	}

	public void EnableParticleSystem()
	{
		if (!_radioactivePs.enableEmission)
		{
			_radioactivePs.enableEmission = true;
			_surfacePs.enableEmission = false;
		}
	}

	#endregion
}
