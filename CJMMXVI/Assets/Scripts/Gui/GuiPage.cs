using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class GuiPage : MonoBehaviour
{
	#region Fields
	[SerializeField]
	Canvas canvas;

	bool hidden;
	#endregion
	
	#region Properties	
	public bool Hidden
	{
		get { return canvas == null ? true : !canvas.enabled; }
		set
		{
			if(canvas != null)
			{
				canvas.enabled = !value;
			}
		}
	}
	#endregion

	#region Methods
	public virtual void OnGameDataChanged(GameData gameData) {}
	#endregion
}
