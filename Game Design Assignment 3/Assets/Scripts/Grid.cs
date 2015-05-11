﻿using System;
using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{

    private GameObject[] cubes;
    public float GridSize;
	// Use this for initialization
	void Start ()
	{
	    ForeceGrid();
        ForeceGrid("WallCube");
        ForeceGrid("GridCell");
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

    public void ForeceGrid(String tag = "Cube")
    {
        cubes = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject cube in cubes)
        {
            Vector3 oldPos = cube.transform.position;
            Vector3 newPos = new Vector3(Mathf.RoundToInt(oldPos.x / GridSize) * GridSize, oldPos.y, Mathf.RoundToInt(oldPos.z / GridSize) * GridSize);

            cube.transform.position = newPos;
        }
    }
}
