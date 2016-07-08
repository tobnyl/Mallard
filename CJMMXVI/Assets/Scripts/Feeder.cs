using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UI = UnityEngine.UI;

public class Feeder : Entity
{
	#region Types
	public enum Kind
	{
		Player,
		Npc,
		Manual,
		Auto,
	}
	#endregion

	#region Fields
	[SerializeField]
	public Kind kind;
	[SerializeField]
	public bool playerControlled;
	[SerializeField]
	public bool autoFeed;
	[SerializeField]
	public bool usesAmmo;
	[SerializeField, ReadOnly]
	public int ammo;
	[SerializeField]
	public Transform feedOrigin;
	[SerializeField]
	public Animator animator;
	[SerializeField]
	public Animator additionalAnimator;
	[SerializeField]
	public float throwDelayDuration;
	[SerializeField]
	public RectTransform Bar;
	[SerializeField]
	public Canvas ReloadCanvas;
	public UI.Button ReloadButton;

	[ReadOnly]
	public float feedTimer;
	[ReadOnly]
	public float throwDelayTimer;

	[ReadOnly] public bool wasButtoned;
	#endregion

	#region Methods
	void OnEnable()
	{
		EntityManager.RegisterEntity(this);

		if (ReloadButton != null)
		{
			ReloadButton.onClick.AddListener(() =>
			{
				wasButtoned = true;
			});
		}
	}

	void OnDisable()
	{
		EntityManager.UnregisterEntity(this);
	}
	#endregion
}