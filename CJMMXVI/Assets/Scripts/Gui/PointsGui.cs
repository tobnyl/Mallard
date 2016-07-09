using UnityEngine;
using UE = UnityEngine;
using UI = UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class PointsGui : MonoBehaviour
{
	#region Fields
	[SerializeField]
	public UI.Text pointsLabel;
	#endregion
	
	#region Properties	
	#endregion

	#region Methods
	public void SetPoints(int points)
	{
		if(pointsLabel != null)
		{
			pointsLabel.text = string.Format("{0} Quacks", points);
		}
	}
	#endregion
}
