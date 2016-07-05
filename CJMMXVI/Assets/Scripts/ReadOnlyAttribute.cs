using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyAttribute : PropertyAttribute
{
	public readonly bool duringPlaytime;

	public ReadOnlyAttribute()
		: this(onlyDuringPlaytime: false)
	{
	}

	public ReadOnlyAttribute(bool onlyDuringPlaytime)
	{
		this.duringPlaytime = onlyDuringPlaytime;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var attr = (ReadOnlyAttribute)attribute;
		bool enabled = false;

		if(attr.duringPlaytime)
		{
			enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
		}

		bool before = GUI.enabled;
		GUI.enabled = enabled;

		EditorGUI.PropertyField(position, property);

		GUI.enabled = before;
	}
}
#endif
