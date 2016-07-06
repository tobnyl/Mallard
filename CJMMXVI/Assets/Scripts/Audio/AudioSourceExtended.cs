using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioSourceExtended : MonoBehaviour
{
    #region Fields
    #endregion

    #region Properties	

	public AudioSource Source { get; set; }
    public float Duration { get; set; }
    public bool Loop { get; set; }

    #endregion

    #region Methods
    public void Run()
	{
        if (!Loop)
        {
            Invoke("Destroy", Duration);
        }
    }

    void Destroy()
    {
		AudioManager.Instance.Return(this);
		//Destroy(gameObject);
    }

    #endregion
}
