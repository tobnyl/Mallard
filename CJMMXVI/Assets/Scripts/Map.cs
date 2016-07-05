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
    [SerializeField]
    public GameObject WaterFallPrefab;

    [Header("Colors")]
	[SerializeField]
    public Color GrassColor;
	[SerializeField]
    public Color WaterColor;

    private int _gridSize = 1;
    private GameObject _cube;
    private int _width;
    private int _height;
    private float _waterfallOffset;

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public void Setup()
    {
        _waterfallOffset = 0.51f;

	    if (SourceMap != null)
	    {
            var cubePosition = Vector3.zero;
            var currentOffset = Vector3.zero;
	        _width = SourceMap.width;
	        _height = SourceMap.height;

	        var pixels = SourceMap.GetPixels(0, 0, _width, _height);

            foreach (var pixel in pixels)
            {
                if (pixel == WaterColor)
                {
                    InstansiateCube(WaterPrefab, cubePosition);

                    AddWaterfallParticleSystem(currentOffset);
                }
                else if (pixel == GrassColor)
                {
                    InstansiateCube(GrassPrefab, cubePosition);
                }

                currentOffset.x += _gridSize;

                if (currentOffset.x > _width - 1)
                {
                    currentOffset.x = 0;
                    currentOffset.z += _gridSize;
                }

                cubePosition = currentOffset;
            }
	    }
	}

    private void AddWaterfallParticleSystem(Vector3 currentOffset)
    {
        

        if (currentOffset.x == 0)
        {
            Debug.Log("Left edge...");
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(-_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180));
        }
        else if (currentOffset.x == _width - 1)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180));
        }
        if (currentOffset.z == 0)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(0, 0, -_waterfallOffset), Quaternion.Euler(0, -180, -180));

        }
        else if (currentOffset.z == _height - 1)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(0, 0, _waterfallOffset), Quaternion.Euler(0, -180, -180));
        }
    }

    private void InstansiateWaterFall(GameObject prefab, Vector3 currentCubeOffset, Vector3 relativeWaterFalloffset, Quaternion rotation)
    {
        var waterFall = Instantiate(WaterFallPrefab, currentCubeOffset, Quaternion.identity) as GameObject;
        waterFall.transform.rotation = rotation;
        waterFall.transform.position += relativeWaterFalloffset;
        waterFall.transform.parent = _cube.transform;
    }

    private void InstansiateCube(GameObject prefab, Vector3 position)
    {
        _cube = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        _cube.transform.parent = transform;
    }
    #endregion
}
