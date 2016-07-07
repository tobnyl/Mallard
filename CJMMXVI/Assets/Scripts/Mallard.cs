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
    [SerializeField]
    public Vector3 Velocity;

    private bool _isTurning;
    private Quaternion _targetRotation;
    private float _speed;

    #endregion

    public bool RotateToTarget(Vector3 direction)
    {
        if (!_isTurning)
        {
            _targetRotation = Quaternion.Euler(0.0f, direction.ToAngleXZ(), 0.0f);
			//Debug.Log("Start turning...");
            _isTurning = true;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 10f);
			//Debug.Log("Is rotating...");

            if (Quaternion.Angle(transform.rotation, _targetRotation) < 5f)
            {
				//Debug.Log("Reached rotation target...");
                transform.rotation = _targetRotation;
                _isTurning = false;
                return true;
            }
        }

        return false;
    }

	//public bool MoveToTarget(Vector3 dir)
	//{
	//	// if vel < speed then vel += acc * dt
	//	if (Velocity.magnitude < gameData.mallard.speed)
	//	{

	//	}
	//}
}
