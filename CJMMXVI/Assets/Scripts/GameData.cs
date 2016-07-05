using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct GameData
{
	#region Types
	[Serializable]
	public struct ManData
	{
	}

	[Serializable]
	public struct MallardData
	{
	}

	[Serializable]
	public struct EnvData
	{
	}
	#endregion

	#region Fields
	[SerializeField]
	public ManData man;
	[SerializeField]
	public MallardData mallard;
	[SerializeField]
	public EnvData environment;
	#endregion

	#region Methods
	#endregion
}
