using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TestAudio : MonoBehaviour
{
    #region Fields

    [Serializable]
    private class Sounds
    {
        public Audio Duck;
        public Audio[] DuckList;
    }

    [SerializeField]
    private Sounds sounds;


	#endregion
	
	#region Properties	
	#endregion

	#region Methods

    void Start()
    {
        //StartCoroutine(DuckTest());
    }

	void OnEnable()
	{
        AudioManager.Instance.Play(sounds.Duck, transform.position);
    }
	
	void Update()
	{
        
    }

    IEnumerator DuckTest()
    {
        for (int i = 0; i < 100; i++)
        {
            AudioManager.Instance.Play(sounds.Duck, transform.position);
            yield return new WaitForSeconds(1);
        }
    }

	#endregion
}
