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

    public float Duration { get; set; }
    public bool Loop { get; set; }

    #endregion

    #region Methods
    void Start()
	{
        if (!Loop)
        {
            Invoke("Destroy", Duration);
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
}
