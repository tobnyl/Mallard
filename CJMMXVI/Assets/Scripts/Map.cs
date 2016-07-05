using UnityEngine;
using UE = UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : MonoBehaviour
{
    public Texture2D SourceMap;
    public GameObject GrassPrefab;
    public GameObject WaterPrefab;

    public Color GrassColor;
    public Color WaterColor;
    public int GridSize = 1;

    private bool _isCOol;
    private GameObject _cube;

	void Start()
	{
	    GameObject currentPrefab;

	    if (SourceMap != null)
	    {
            Vector3 cubePosition = Vector3.zero;
            Vector3 currentOffset = Vector3.zero;
	        var width = SourceMap.width;
	        var height = SourceMap.height;
	        int y = 0;

	        var pixels = SourceMap.GetPixels(0, 0, width, height);

            for (int i = 0; i < pixels.Length - 1; i++)
            {
                Debug.Log(pixels[i]);
                InstansiateCube(WaterPrefab, cubePosition);

                currentOffset.x += GridSize;

                if (currentOffset.x > width)
                {
                    currentOffset.x = 0;
                    currentOffset.z += GridSize;
                }

                cubePosition = currentOffset;

                //if (pixels[i].Equals(WaterColor))
                //{
                //    InstansiateCube(WaterPrefab);
                //}


            }

	        //for (int i = 0; i < pixels.Length; i++)
	        //{
         //       cube = Instantiate(GrassPrefab, transform.position, Quaternion.identity) as GameObject;

         //    //   if (pixels[i] == GrassColor)
	        //    //{
	        //    //    cube = Instantiate(GrassPrefab, transform.position, Quaternion.identity) as GameObject;
	        //    //}
         //    //   else if (pixels[i] == WaterColor)
         //    //   {
         //    //       cube = Instantiate(WaterPrefab, transform.position, Quaternion.identity) as GameObject;
         //    //   }
	        //}
	    }
	}

    private void InstansiateCube(GameObject prefab, Vector3 position)
    {
        _cube = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        _cube.transform.parent = transform;
    }

	void Update() {

	}
}
