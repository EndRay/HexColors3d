using UnityEngine;
using System.Collections;

public class HexController : MonoBehaviour
{

    public BigCubesCreator BigCubesCreator;

    public BoardGenerator BoardGenerator;

    public int[,] Hexes = new int[16,16];

	// Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
	void Update () {
	
	}
}
