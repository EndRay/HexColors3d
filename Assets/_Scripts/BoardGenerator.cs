﻿using UnityEngine;

public class BoardGenerator : MonoBehaviour
{

    public HexController HexController;

    public BigCubesCreator BigCubesCreator;

    public GameObject[,] ObjHexes = new GameObject[16,16];

    public Material[] Materials;

    public GameObject Hex;

    // Use this for initialization
	void Start () {
        LoadLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadLevel()
    {
        BigCubesCreator.SetEight();
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                if (x == 0 || y == 0 || x == 15 || y == 15)
                {
                    HexController.Hexes[x, y] = 8;
                    continue;
                }
                int r = Random.Range(0, 6);
                CreateHex(r, x, y);
            }
        }
        SetHex(6, 1, 1);
        HexController.BorderP1[1,1] = 1;
        SetHex(7,14,14);
        HexController.BorderP2[14, 14] = 1;
        BigCubesCreator.CreateBigCube();
    }

    private void CreateHex(int color, int x, int y)
    {
        GameObject createObj = Instantiate(Hex, new Vector3(-y / 2 + x, y, x + (y + 1) / 2), Quaternion.identity) as GameObject;
        ObjHexes[x, y] = createObj;
        SetHex(color, x, y);
    }

    public void SetHex(int color, int x, int y)
    {
        ObjHexes[x, y].GetComponent<Renderer>().material = Materials[color];
        HexController.Hexes[x, y] = color;
        BigCubesCreator.BigCubesCreatorColor[-y / 2 + x + 7, y + 1, x + (y + 1) / 2 + 1] = color;
        BigCubesCreator.BigCubesCreatorObj[-y / 2 + x + 7, y + 1, x + (y + 1) / 2 + 1] = ObjHexes[x,y];
    }
}
