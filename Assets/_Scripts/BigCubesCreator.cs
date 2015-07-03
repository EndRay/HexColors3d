using UnityEngine;

public class BigCubesCreator : MonoBehaviour {

    public HexController HexController;

    public BoardGenerator BoardGenerator;

    public int[,,] BigCubesCreatorColor = new int[23,18,24];

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateBigCube()
    {
        for (int x = 1; x < 21; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                for (int z = 1; z < 22; z++)
                {

                }
            }  
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
}
