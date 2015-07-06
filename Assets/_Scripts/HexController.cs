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

    public int LastMoveColor = 6;

    public int PenultimateMoveColor = 6;

    public GameObject[,] DisabledButtons = new GameObject[6, 2];

    public GameObject[] DisabledButtonsP1 = new GameObject[6];

    public GameObject[] DisabledButtonsP2 = new GameObject[6];

    public GameObject[,] Buttons = new GameObject[6, 2];

    public GameObject[] ButtonsP1 = new GameObject[6];

    public GameObject[] ButtonsP2 = new GameObject[6];

    public bool IsWin; 

    // Use this for initialization
    private void Start()
    {
        for (int c = 0; c < 6; c++)
        {
            DisabledButtons[c, 0] = DisabledButtonsP1[c];
            DisabledButtons[c, 1] = DisabledButtonsP2[c];
        }
        for (int c = 0; c < 6; c++)
        {
            Buttons[c, 0] = ButtonsP1[c];
            Buttons[c, 1] = ButtonsP2[c];
        }
        BoardGenerator.LoadLevel();
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
        if (color == LastMoveColor||color == PenultimateMoveColor)
        {
            return;
        }
        if (p1Move == P1Move)
        {
            HexSet(color);
            P1Move = !P1Move;
            PenultimateMoveColor = LastMoveColor;
            LastMoveColor = color;
            ButtonsActive();
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
        BigCubesCreator.CreateBigCube();
        IsGameEnd();
        HaveMoves(6);
        HaveMoves(7);
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

    public void ButtonsActive()
    {
        for (int c = 0; c < 6; c++)
        {
            for (int p = 0; p < 2; p++)
            {
                Buttons[c, p].SetActive(true);
                if (IsWin)
                {
                    DisabledButtons[c, p].SetActive(false);
                    Buttons[c, p].SetActive(false);
                }
                DisabledButtons[c,p].SetActive(false);
                if ((c == LastMoveColor || c == PenultimateMoveColor) || p != (P1Move ? 0 : 1))
                {
                    DisabledButtons[c,p].SetActive(true);
                }
            }
        }
    }

    public void IsGameEnd()
    {
        for (int x = 1; x < 15; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                if (Hexes[x, y] != 6 && Hexes[x, y] != 7)
                {
                    return;
                }
            }
        }
        IsWin = true;
        ButtonsActive();
        BigCubesCreator.Culculate();
    }

    public void HaveMoves(int playerChangeColor)
    {
        var border = playerChangeColor == 6 ? BorderP2 : BorderP1;
        for (int x = 1; x < 15; x++)
        {
            for (int y = 1; y < 15; y++)
            {
                if (border[x,y] == 0)
                {
                    continue;                    
                }
                foreach (var s in _sides)
                {
                    if (Hexes[x + s[0], y + s[1]] < 6)
                    {
                        return;
                    }
                }

                foreach (var rS in _rSides)
                {
                    int dx = y % 2 == 0 ? -rS[0] : rS[0];
                    if (Hexes[x + dx, y + rS[1]] < 6)
                    {
                        return;
                    }
                }
            }
        }
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                if (Hexes[x, y] < 6)
                {
                    BoardGenerator.SetHex(playerChangeColor,x,y);
                }
            }
        }
    }
}