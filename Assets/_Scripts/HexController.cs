using UnityEngine;

public class HexController : MonoBehaviour
{

    private readonly int[][] _sides =
    {
        new[] {0,1},
        new[] {1,0},
        new[] {0,-1},
        new[] {-1,0}
    };

    private readonly int[][] _rSides =
    {
        new[] {1, 1},
        new[] {1, -1}
    };
    public BigCubesCreator BigCubesCreator;

    public BoardGenerator BoardGenerator;

    public int[,] Hexes = new int[16,16];

    public int[,] BorderP1 = new int[16,16]; 

	public int[,] BorderP2 = new int[16,16];
    
    public bool P1Move;

    public int LastMoveColor;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
	void Update () {
	
	}

    public void PressButton(int buttonColor)
    {
        int color;
        bool p1Move;
        if (buttonColor >= 0)
        {
            color = buttonColor;
            p1Move = true;
        }
        else
        {
            color = -(buttonColor + 1);
            p1Move = false;
        }
        if (color == LastMoveColor)
        {
            return;
        }
        if (p1Move == P1Move)
        {
            HexSet(color);
            P1Move = !P1Move;
            LastMoveColor = color;
        }        
    }

    public void HexSet(int color)
    {
        var playerMove = P1Move ? 6 : 7;
        bool isSet = false;
        for (int x = 1; x < 15; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                var border = P1Move ? BorderP1[x, y] : BorderP2[x, y];
                if (border == 1)
                {
                    foreach (var s in _sides)
                    {
                        if (Hexes[x + s[0], y + s[1]] == color)
                        {
                            BoardGenerator.SetHex(playerMove, x + s[0], y + s[1]);
                            isSet = true;
                        }
                    }
                    foreach (var rS in _rSides)
                    {
                        int dx = y%2 == 0 ? -rS[0]: rS[0];
                        
                        if (Hexes[x + dx, y + rS[1]] == color)
                        {
                            BoardGenerator.SetHex(playerMove, x + dx, y + rS[1]);
                            isSet = true;
                        }
                    }
                }
            }
        }
        if (isSet)
        {
            SetBorder();
            HexSet(color);
        }
    }

    private void SetBorder()
    {
        int playerMove;
        int[,] border;
        if (P1Move)
        {
            playerMove = 6;
            border = BorderP1;
        }
        else
        {
            playerMove = 7;
            border = BorderP2;
        }
        for (int x = 1; x < 15; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                if (Hexes[x, y] == playerMove)
                {
                    border[x, y] = IsBorder(x, y, playerMove) ? 1 : 0;
                }
            }
        }
    }

    private bool IsBorder(int x, int y, int playerMove)
    {
        foreach (var s in _sides)
        {
            if (Hexes[x + s[0], y + s[1]] != playerMove)
            {
                return true;
            }
        }

        foreach (var rS in _rSides)
        {
            int dx = y%2 == 0 ? -rS[0] : rS[0];
            if (Hexes[x + dx, y + rS[1]] != playerMove)
            {
                return true;
            }
        }
        return false;
    }
}