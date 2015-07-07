using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BigCubesCreator : MonoBehaviour {

    private readonly int[][] _cubeSides =
    {
        new[]{-1,1,0},
        new[]{0,1,1}
    };

    public HexController HexController;

    public BoardGenerator BoardGenerator;

    public int[,,] BigCubesCreatorColor = new int[23, 18, 24];

    public GameObject[,,] BigCubesCreatorObj = new GameObject[23, 18, 24];

    public int P1Score;
    
    public int P2Score;

    public GameObject P1ScoreText;

    public GameObject P2ScoreText;

    public GameObject PContainer;

    public GameObject CreateContainerP1;

    public GameObject CreateContainerP2;

    public Tween RotatorP1;

    public Tween RotatorP2;

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
                        if (!isSet)
                        {
                            isSet = SetBigCube(x, y + 1, z, color);
                        }
                        else
                        {
                            SetBigCube(x, y + 1, z, color);
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
                    BigCubesCreatorObj[x, y, z] = null;
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
        if (BigCubesCreatorColor[x, y, z] != 8)
        {
            BigCubesCreatorObj[x, y, z].GetComponent<Renderer>().material = BoardGenerator.Materials[color];
            BigCubesCreatorColor[x, y, z] = color;
            return true;
        }
        BigCubesCreatorColor[x, y, z] = color;
        var createObj = Instantiate(BoardGenerator.Hex, new Vector3(x - 7, y - 1, z - 1), Quaternion.identity) as GameObject;
        createObj.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From(); 
        createObj.GetComponent<Renderer>().material = BoardGenerator.Materials[color];
        BigCubesCreatorObj[x, y, z] = createObj;
        return true;
    }

    public void Culculate()
    {
        P1Score = 0;
        P2Score = 0;
        Vector3 centerPosP1 = new Vector3();
        Vector3 centerPosP2 = new Vector3();
        for (int x = 0; x < 23; x++)
        {
            for (int y = 0; y < 18; y++)
            {
                for (int z = 0; z < 24; z++)
                {
                    if (BigCubesCreatorColor[x,y,z] == 6)
                    {
                        P1Score++;
                        centerPosP1 += BigCubesCreatorObj[x, y, z].transform.position;
                    }
                    if (BigCubesCreatorColor[x,y,z] == 7)
                    {
                        P2Score++; 
                        centerPosP2 += BigCubesCreatorObj[x, y, z].transform.position;
                    }
                }   
            }
        }
        P1ScoreText.SetActive(true);
        P1ScoreText.GetComponent<TextMesh>().text = P1Score.ToString();
        P2ScoreText.SetActive(true);
        P2ScoreText.GetComponent<TextMesh>().text = P2Score.ToString();
        centerPosP1 /= P1Score;
        centerPosP2 /= P2Score;
        CreateContainerP1 = Instantiate(PContainer, centerPosP1, Quaternion.identity) as GameObject;
        CreateContainerP1.name = "P1Container";
        CreateContainerP2 = Instantiate(PContainer, centerPosP2, Quaternion.identity) as GameObject;
        CreateContainerP2.name = "P2Container";
        for (int x = 0; x < 23; x++)
        {
            for (int y = 0; y < 18; y++)
            {
                for (int z = 0; z < 24; z++)
                {
                    if (BigCubesCreatorColor[x, y, z] == 6)
                    {
                        BigCubesCreatorObj[x, y, z].transform.SetParent(CreateContainerP1.transform);
                    }
                    if (BigCubesCreatorColor[x, y, z] == 7)
                    {
                        BigCubesCreatorObj[x, y, z].transform.SetParent(CreateContainerP2.transform);
                    }
                }
            }
        }
        CreateContainerP1.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 6).SetEase(Ease.OutElastic);
        RotatorP1 = CreateContainerP1.transform.DORotate(new Vector3(40, 30, 20), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        CreateContainerP1.transform.DOMove(new Vector3(2, 15, 4.5f), 2);
        P1ScoreText.transform.position = new Vector3(2, 9, 4.5f);
        CreateContainerP2.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 6).SetEase(Ease.OutElastic);
        RotatorP2 = CreateContainerP2.transform.DORotate(new Vector3(-40, -30, -20), 3, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        CreateContainerP2.transform.DOMove(new Vector3(10, 11.5f, 15), 2);
        P2ScoreText.transform.position = new Vector3(10, 5.5f, 15);
    }
}
