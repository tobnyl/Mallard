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

    [Header("Waterfall")]
    [SerializeField]
    public GameObject WaterFallPrefab;
    [SerializeField]
    public GameObject WaterFallBackgroundPrefab;

    public float WaterFallHeight;

    private int _gridSize;
    private int _width;
    private int _height;
    private float _waterfallOffset;
    private float _waterfallPlaneOffset;

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public void Setup()
    {
        _gridSize = 1;
        _waterfallOffset = 0.61f;
        _waterfallPlaneOffset = 0.5f;
        

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
                else if (pixel.r == 0 && pixel.b == 0)
                {
                    var cubeHeight = GetCubeHeight(pixel.g);

                    Debug.Log("G: " + pixel.g + " | " + cubeHeight);

                    InstansiateCube(GrassPrefab, cubePosition, cubeHeight);
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

    private int GetCubeHeight(float range)
    {
        return Convert.ToInt32(range*10);
    }

    private void AddWaterfallParticleSystem(Vector3 currentOffset, GameObject cube)
    {
        var waterFallOffset = currentOffset + new Vector3(0, _gridSize/2f, 0);

        // TODO: enable Left- and top-edge if we implement camera rotation

        // Left edge
        //if (currentOffset.x == 0)
        //{
        //    InstansiateWaterFall(WaterFallPrefab, waterFallOffset, new Vector3(-_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180), cube.transform);
        //    InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(-_waterfallPlaneOffset, -1, 0),  Quaternion.Euler(0, 0, 90), cube.transform);
        //}
        // Right edge
        if (currentOffset.x == _width - 1)
        {
            InstansiateWaterFall(WaterFallPrefab, waterFallOffset, new Vector3(_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180), cube.transform);
            InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(_waterfallPlaneOffset, -1, 0), Quaternion.Euler(0, 0, -90), cube.transform);
        }
        // Bottom edge
        else if (currentOffset.z == 0)
        {
            InstansiateWaterFall(WaterFallPrefab, waterFallOffset, new Vector3(0, 0, -_waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
            InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, -_waterfallPlaneOffset), Quaternion.Euler(-90, 0, 0), cube.transform);
        }
        // Top edge
        //else if (currentOffset.z == _height - 1)
        //{
        //    InstansiateWaterFall(WaterFallPrefab, waterFallOffset, new Vector3(0, 0, _waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
        //    InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, _waterfallPlaneOffset), Quaternion.Euler(90, 0, 0), cube.transform);
        //}
    }

    private void InstansiateWaterFall(GameObject prefab, Vector3 position, Vector3 relativeOffset, Quaternion rotation, Transform parent)
    {
        var waterFall = Instantiate(prefab, position, rotation) as GameObject;                
        waterFall.transform.position += relativeOffset;
        waterFall.transform.parent = parent;

        var waterFallParticleSystem = waterFall.GetComponent<ParticleSystem>();
        waterFallParticleSystem.startLifetime = WaterFallHeight - 2;
    }

    private void InstansiateWaterFallBackground(GameObject prefab, Vector3 position, Vector3 relativeOffset, Quaternion rotation, Transform parent)
    {
        var waterFall = Instantiate(prefab, position, rotation) as GameObject;
        waterFall.transform.position += relativeOffset + new Vector3(0, -WaterFallHeight/2f + _gridSize/2f, 0);
        waterFall.transform.parent = parent;

        var scale = waterFall.transform.localScale;
        scale.x *= WaterFallHeight;
        waterFall.transform.localScale = scale;

        // This is nice code, yes...
        if (rotation.eulerAngles.x != 0)
        {
            waterFall.transform.Rotate(0, 90, 0);
        }
    }

    private GameObject InstansiateCube(GameObject prefab, Vector3 position, int height = 1)
    {
        GameObject cube = null;

        if (height > 1)
        {
            for (var i = 1; i < height; i++)
            {
                position += new Vector3(0, 1, 0);

                cube = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
                cube.transform.parent = transform;
            }
        }
        else
        {
            cube = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
            cube.transform.parent = transform;
        }

        // Only used when height = 1
        return cube;
    }
    #endregion
}
