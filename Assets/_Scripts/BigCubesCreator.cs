using UnityEngine;

public class BigCubesCreator : MonoBehaviour {

    private readonly int[][] _cubeSides =
    {
        new[]{-1,1,0},
        new[]{0,1,1}
    };

    private readonly int[][] _setSides =
    {
        new []{0,1,0},
        new []{-1,0,0},
        new []{0,0,1},
        new []{-1,0,1},
        new []{-1,1,1}
    };

    public HexController HexController;

    public BoardGenerator BoardGenerator;

    public int[,,] BigCubesCreatorColor = new int[23, 18, 24];

    public GameObject[,,] BigCubesCreatorObj = new GameObject[23, 18, 24];

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateBigCube()
    {
        bool isSet = false;
        for (int x = 1; x < 21; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                for (int z = 1; z < 22; z++)
                {
                    bool isBigCube = true;
                    if (BigCubesCreatorColor[x, y, z] == 8)
                        continue;
                    int color = BigCubesCreatorColor[x, y, z];
                    foreach (var s in _cubeSides)
                    {
                        if (BigCubesCreatorColor[x + s[0], y + s[1], z + s[2]] != color)
                        {
                            isBigCube = false;
                        }
                    }
                    if (isBigCube)
                    {
                        foreach (var sS in _setSides)
                        {
                            if (!isSet)
                            {
                                isSet = SetBigCube(x + sS[0], y + sS[1], z + sS[2], color);
                            }
                            else
                            {
                                SetBigCube(x + sS[0], y + sS[1], z + sS[2], color);
                            } 
                        }
                    }
                }
            }  
        }
        if (isSet)
        {
            CreateBigCube();
        }
    }

    public void SetEight()
    {
        for (int x = 0; x < 23; x++)
        {
            for (int y = 0; y < 18; y++)
            {
                for (int z = 0; z < 24; z++)
                {
                    BigCubesCreatorColor[x, y, z] = 8;
                }
            }
        }
    }

    private bool SetBigCube(int x, int y, int z, int color)
    {
        if (BigCubesCreatorColor[x, y, z] == color)
        {
            return false;
        }
        if (BigCubesCreatorObj[x, y, z] != null)
        {
            Destroy(BigCubesCreatorObj[x, y, z]);
            BigCubesCreatorObj[x, y, z] = null;
            SetBigCube(x, y, z, color);
        }
        BigCubesCreatorColor[x, y, z] = color;
        var createObj = Instantiate(BoardGenerator.Hex, new Vector3(x - 7, y - 1, z - 1), Quaternion.identity) as GameObject;
        createObj.GetComponent<Renderer>().material = BoardGenerator.Materials[color];
        BigCubesCreatorObj[x, y, z] = createObj;
        return true;
    }
}
