using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class BoardGenerator : MonoBehaviour
{

    public HexController HexController;

    public BigCubesCreator BigCubesCreator;

    public GameObject[,] ObjHexes = new GameObject[16,16];

    public Material[] Materials;

    public GameObject Hex;

    private Vector3 _xPos = new Vector3(1,0,-1);

    private Vector3 _yPos = new Vector3(0,1,0);
    
    private Vector3 _xPosAdd = new Vector3(0,0,1);

    // Use this for initialization
	void Start () {
        LoadLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadLevel()
    {
        for (int x = 0; x < 16; x++)
        {
              for (int y = 0; y < 16; y++)
              {
                if (x == 0 || y == 0 || x == 15 || y == 15)
                {
                    HexController.Hexes[x, y] = 8;
                    continue;
                }
                int R = Random.Range(0,6);
                if(x==1 && y==1)
                {
                    R = 6;
                }
                if (x==14 && y==14)
                {
                    R = 7;
                }
                var createObj = Set(R,new Vector3(-y/2 + x, y, x + (y+1)/2));
                ObjHexes[x, y] = createObj;
                HexController.Hexes[x,y] = R;
            }
        }
    }

    private GameObject Set(int color, Vector3 vector)
    {
        GameObject createObj = Instantiate(Hex, vector,Quaternion.identity) as GameObject;
        createObj.GetComponent<Renderer>().material = Materials[color];
        return createObj;
    }
}
