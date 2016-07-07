using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Mallard : Entity
{
	#region Fields
	[SerializeField, ReadOnly]
	public Vector3 defaultPosition;
	[SerializeField, ReadOnly]
	public Food targetFood;
	[SerializeField, ReadOnly]
	public float eatTimer;

    private bool _isTurning;
    private Quaternion _targetRotation;

    #endregion

    public bool RotateToTarget(Vector3 direction)
    {
        if (!_isTurning && targetFood != null)
        {
            _targetRotation = Quaternion.Euler(0.0f, direction.ToAngleXZ(), 0.0f);
            _isTurning = true;
        }
        else if (targetFood != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 10f);

            if (Quaternion.Angle(transform.rotation, _targetRotation) < 5f)
            {
                transform.rotation = _targetRotation;
                _isTurning = false;
                return true;
            }
        }

        return false;
    }
}
