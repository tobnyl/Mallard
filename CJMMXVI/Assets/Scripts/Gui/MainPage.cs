using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainPage : GuiPage
{
	#region Fields
	[SerializeField]
	public PointsGui points;
	[SerializeField]
	public UI.Button upgradesButton;
	#endregion
}
