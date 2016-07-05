using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Feeder : Entity
{
	#region Fields
	[SerializeField]
	public bool playerControlled;
	[SerializeField]
	public bool autoFeed;

	float feedCooldown;
	float feedTimer;
	#endregion
	
	#region Properties
	public bool CanFeed
	{
		get { return feedCooldown <= 0.0f; }
	}
	#endregion

	#region Methods
	public void DoUpdate()
	{
		if(feedTimer > 0.0f)
		{
			feedTimer -= Time.deltaTime;
		}
		else if(autoFeed)
		{
			Feed();
		}
	}

	public void Feed()
	{
		if(!CanFeed) { return; }

		feedTimer = feedCooldown;
	}
	#endregion
}
