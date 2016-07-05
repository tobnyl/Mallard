﻿using UnityEngine;
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

    [Header("Waterfall")]
    [SerializeField]
    public GameObject WaterFallPrefab;
    [SerializeField]
    public GameObject WaterFallBackgroundPrefab;

    private int _gridSize;
    private int _width;
    private int _height;
    private float _waterfallOffset;

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public void Setup()
    {
        _gridSize = 1;
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
                    var cube = InstansiateCube(WaterPrefab, cubePosition);

                    AddWaterfallParticleSystem(currentOffset, cube);
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

    private void AddWaterfallParticleSystem(Vector3 currentOffset, GameObject cube)
    {
        // Left edge
        if (currentOffset.x == 0)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(-_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180), cube.transform);
            InstansiateWaterFall(WaterFallBackgroundPrefab, currentOffset, new Vector3(-_waterfallOffset, -1, 0),  Quaternion.Euler(0, 0, 90), cube.transform);
        }
        // Right edge
        else if (currentOffset.x == _width - 1)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180), cube.transform);
            InstansiateWaterFall(WaterFallBackgroundPrefab, currentOffset, new Vector3(_waterfallOffset, -1, 0), Quaternion.Euler(0, 0, -90), cube.transform);
        }
        // Bottom edge
        if (currentOffset.z == 0)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(0, 0, -_waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
            InstansiateWaterFall(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, -_waterfallOffset), Quaternion.Euler(-90, 0, 0), cube.transform);
        }
        // Top edge
        else if (currentOffset.z == _height - 1)
        {
            InstansiateWaterFall(WaterFallPrefab, currentOffset, new Vector3(0, 0, _waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
            InstansiateWaterFall(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, _waterfallOffset), Quaternion.Euler(90, 0, 0), cube.transform);
        }
    }

    private void InstansiateWaterFall(GameObject prefab, Vector3 position, Vector3 relativeOffset, Quaternion rotation, Transform parent)
    {
        var waterFall = Instantiate(prefab, position, rotation) as GameObject;        
        waterFall.transform.position += relativeOffset;
        waterFall.transform.parent = parent;

    }

    private void InstansiateWaterFallBackground(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        var waterFallBackground = Instantiate(prefab, position, rotation) as GameObject;
        waterFallBackground.transform.position += new Vector3(0, -1, 0);
        waterFallBackground.transform.parent = parent;
    }

    private GameObject InstansiateCube(GameObject prefab, Vector3 position)
    {
        var cube = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        cube.transform.parent = transform;

        return cube;
    }
    #endregion
}
