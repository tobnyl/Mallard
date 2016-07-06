using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradeButtonGui : MonoBehaviour
{
	#region Fields
	[SerializeField]
	public UI.Button button;
	[SerializeField]
	UI.Text titleLabel;

	[SerializeField, ReadOnly]
	public Upgrade upgrade;
	#endregion

	#region Methods
	public void SetTitle(string title)
	{
		titleLabel.text = title;
	}
	#endregion
}
