using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : MonoBehaviour
{
    #region Fields 
 	[SerializeField]
    public Texture2D SourceMap;
	[SerializeField]
	public GameObject GrassPrefab;
	[SerializeField]
    public GameObject WaterPrefab;

    [Header("Colors")]
	[SerializeField]
    public Color GrassColor;
	[SerializeField]
    public Color WaterColor;

    private int _gridSize = 1;
    private GameObject _cube;

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public void Setup()
	{
	    if (SourceMap != null)
	    {
            var cubePosition = Vector3.zero;
            var currentOffset = Vector3.zero;
	        var width = SourceMap.width;
	        var height = SourceMap.height;

	        var pixels = SourceMap.GetPixels(0, 0, width, height);

            foreach (Color pixel in pixels)
            {
                if (pixel == WaterColor)
                {
                    InstansiateCube(WaterPrefab, cubePosition);
                }
                else if (pixel == GrassColor)
                {
                    InstansiateCube(GrassPrefab, cubePosition);
                }

                currentOffset.x += _gridSize;

                if (currentOffset.x > width - 1)
                {
                    currentOffset.x = 0;
                    currentOffset.z += _gridSize;
                }

                cubePosition = currentOffset;
            }
	    }
	}

    private void InstansiateCube(GameObject prefab, Vector3 position)
    {
        _cube = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        _cube.transform.parent = transform;
    }
    #endregion
}
