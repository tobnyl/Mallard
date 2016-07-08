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
    public GameObject DirtCube;
    [SerializeField]
    public Light LightPrefab;
    [SerializeField]
    public float WaterFallHeight = 10f;
    [SerializeField]
    public float WorldBottomHeight = 10f;

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
    [SerializeField]
    public GameObject WaterFallEdgePrefab;
	[SerializeField]
	public Material WaterMaterial;
	[SerializeField]
	public Material RadioactiveMaterial;

	private int _gridSize;
    private int _width;
    private int _height;
    private float _waterfallOffset;
    private float _waterfallPlaneOffset;
    private bool _isWaterFall;
    private Vector3 _mapCenter;

	private List<WaterCube> _waterCubeList;
	private List<MeshRenderer> _waterFallList; 

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public void Setup()
    {
        _gridSize = 1;
        _waterfallOffset = 0.61f;
        _waterfallPlaneOffset = 0.5f;
		_waterCubeList = new List<WaterCube>();
		_waterFallList = new List<MeshRenderer>();



		if (SourceMap != null)
	    {
            var cubePosition = Vector3.zero;
            var currentOffset = Vector3.zero;
	        _width = SourceMap.width;
	        _height = SourceMap.height;

	        var mapCenterOffset = _width/2f - _gridSize/2f;
            _mapCenter = transform.position + new Vector3(mapCenterOffset, 0, mapCenterOffset);

            var pixels = SourceMap.GetPixels(0, 0, _width, _height);

            foreach (var pixel in pixels)
            {
                if (pixel == WaterColor)
                {
                    _isWaterFall = false;
                    var cube = InstansiateWaterCube(WaterPrefab, cubePosition);

                    AddWaterfallParticleSystem(currentOffset, cube);

                    if (!_isWaterFall)
                    {
                        InstansiateDirtCubes(DirtCube, cubePosition);
                    }
                }
                else if (pixel.r == 0 && pixel.b == 0)
                {
                    var cubeHeight = GetCubeHeight(pixel.g);

                    InstansiateGrassCube(GrassPrefab, DirtCube, cubePosition, cubeHeight);
                    InstansiateDirtCubes(DirtCube, cubePosition);
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

	public void DoUpdate()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			EnableRadioactiveWater();
		}
	}

	public void EnableRadioactiveWater()
	{
		foreach (var waterObject in _waterCubeList)
		{
			waterObject.SetMaterial(RadioactiveMaterial);
			waterObject.EnableParticleSystem();
		}

		foreach (var waterFall in _waterFallList)
		{
			waterFall.material = RadioactiveMaterial;
		}
	}

    private int GetCubeHeight(float range)
    {
        return Mathf.FloorToInt(range * 10f);
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
            InstansiateWaterFall(waterFallOffset, new Vector3(_waterfallOffset, 0, 0), Quaternion.Euler(0, 90, 180), cube.transform);
            InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(_waterfallPlaneOffset, -1, 0), Quaternion.Euler(0, 0, -90), cube.transform);

            _isWaterFall = true;
        }
        // Bottom edge
        else if (currentOffset.z == 0)
        {
            InstansiateWaterFall(waterFallOffset, new Vector3(0, 0, -_waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
            InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, -_waterfallPlaneOffset), Quaternion.Euler(-90, 0, 0), cube.transform);

            _isWaterFall = true;
        }
        // Top edge
        //else if (currentOffset.z == _height - 1)
        //{
        //    InstansiateWaterFall(WaterFallPrefab, waterFallOffset, new Vector3(0, 0, _waterfallOffset), Quaternion.Euler(0, -180, -180), cube.transform);
        //    InstansiateWaterFallBackground(WaterFallBackgroundPrefab, currentOffset, new Vector3(0, -1, _waterfallPlaneOffset), Quaternion.Euler(90, 0, 0), cube.transform);
        //}

    }

    private void InstansiateWaterFall(Vector3 position, Vector3 relativeOffset, Quaternion rotation, Transform parent)
    {
        var relativePosition = position + relativeOffset;

        var waterFall = Instantiate(WaterFallPrefab, relativePosition, rotation) as GameObject;                
        waterFall.transform.parent = parent;		

        var waterFallParticleSystem = waterFall.GetComponent<ParticleSystem>();
        waterFallParticleSystem.startLifetime = WaterFallHeight - 2;

        var waterFallEdge = Instantiate(WaterFallEdgePrefab, relativePosition, rotation) as GameObject;
        waterFallEdge.transform.parent = parent;
    }

    private void InstansiateWaterFallBackground(GameObject prefab, Vector3 position, Vector3 relativeOffset, Quaternion rotation, Transform parent)
    {
        var waterFall = Instantiate(prefab, position, rotation) as GameObject;
        waterFall.transform.position += relativeOffset + new Vector3(0, -WaterFallHeight/2f + _gridSize/2f, 0);
        waterFall.transform.parent = parent;
		_waterFallList.Add(waterFall.GetComponent<MeshRenderer>());

        var scale = waterFall.transform.localScale;
        scale.x *= WaterFallHeight;
        waterFall.transform.localScale = scale;

        // This is nice code, yes...
        if (rotation.eulerAngles.x != 0)
        {
            waterFall.transform.Rotate(0, 90, 0);
        }
    }

    private GameObject InstansiateWaterCube(GameObject prefab, Vector3 position)
    {
        GameObject cube = null;
        var dirtCubePosition = position;

        cube = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
        cube.transform.parent = transform;
		_waterCubeList.Add(cube.GetComponent<WaterCube>());

		return cube;
    }

    private void InstansiateGrassCube(GameObject prefab, GameObject dirtPrefab, Vector3 position, int height = 0)
    {
        GameObject cube = null;

        if (height >= 1)
        {
            cube = Instantiate(dirtPrefab, position, prefab.transform.rotation) as GameObject;
            cube.transform.parent = transform;

            for (var i = 0; i < height; i++)
            {
                position += new Vector3(0, 1, 0);

                var prefabToUse = (i < height - 1 ? dirtPrefab : prefab);

                cube = Instantiate(prefabToUse, position, prefab.transform.rotation) as GameObject;
                cube.transform.parent = transform;
            }
        }
        else
        {
            cube = Instantiate(prefab, position, prefab.transform.rotation) as GameObject;
            cube.transform.parent = transform;
        }
        
    }

    private void InstansiateDirtCubes(GameObject prefab, Vector3 position)
    {
        var distanceFromCenter = _mapCenter - position;
        var maxHeight = Convert.ToInt32(WorldBottomHeight - distanceFromCenter.magnitude);

        var isCubeAtEdge = position.x == _width - 1 || position.x == 0 || position.z == _height - 1 || position.z == 0;

        var startIndex = (isCubeAtEdge ? 1 : maxHeight);

        for (var i = startIndex; i <= maxHeight; i++)
        {
            var dirtCube = Instantiate(DirtCube, position + new Vector3(0, -i, 0), prefab.transform.rotation) as GameObject;
            dirtCube.transform.parent = transform;
        }
    }

    #endregion
}
