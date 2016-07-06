using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameUi : MonoBehaviour
{
	#region Fields
	[SerializeField]
	public MainPage main;
	[SerializeField]
	public UpgradesPage upgrades;
	#endregion
	
	#region Properties	
	#endregion

	#region Methods
	public void Setup(UpgradeManager upgradeMan)
	{
		upgrades.Setup(upgradeMan);
	}
	#endregion
}
