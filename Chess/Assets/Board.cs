using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject squarePrefab;

    public Color lightColor, darkColor;

    /*
    * uppercase letter -> black piece
    * lowercase letter -> white piece
    * / -> end of a row
    * number -> empty spaces
    */
    private const string startPositions = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

    /*
     * assigns a char to every piece 
     */
    private Dictionary<char, int> getPieceTypeFromChar = new Dictionary<char, int>()
        {
            {'r', Pieces.rook}, {'n', Pieces.knight}, {'b', Pieces.bishop}, {'q', Pieces.queen}, {'k', Pieces.king}, {'p', Pieces.pawn}
        };

    /*
     * stores the piece and color of every square
     */
    public static int[,] piecesOnSquares;

    /*
     * stores all the squares
     */
    public static GameObject[] squares;

    void Start()
    {
        piecesOnSquares = new int[64, 2];

        squares = new GameObject[64];

        CreateBoard();

        LoadPositionsFromString(startPositions);
    }

    /*
     * creates a8x8 board with alternately light and dark squares
     */
    void CreateBoard()
    {
        for(int rank = 0; rank < 8; rank++)
        {
            for(int file = 0; file < 8; file++)
            {
                /*
                 * checks if the sum of the rank and file is divisible by two
                 */
                bool isDarkSquare = (rank + file) % 2 == 0;

                /*
                 * sets the position of the current square at the current rank and file
                 * offset of -3.5f to center the board
                 */
                Vector3 position = new Vector3(file - 3.5f, rank - 3.5f, 0);

                /*
                 * adds a square at the calculated position
                 */
                AddSquare(position, isDarkSquare, squarePrefab, rank * 8 + file);
            }
        }
    }

    /*
     * adds a square to the world
     */
    void AddSquare(Vector3 position, bool isDarkSquare, GameObject square, int a)
    {
        /*
         * creates a new instance of the square-prefab and adds it to the array which stores all the squares
         */
        squares[a] = Instantiate(squarePrefab);

        /*
         * sets the position of the current square to the given position
         */
        squares[a].transform.position = position;

        /*
         * sets the color of the current square to dark or light
         */
        squares[a].GetComponent<SpriteRenderer>().color = (isDarkSquare) ? darkColor : lightColor;
    }

    /*
     * moves a piece in the piece-array
     * buggy af
     */
    public static void movePieceOnSquare(int startPosition, int endPosition)
    {
        piecesOnSquares[endPosition, 0] = piecesOnSquares[startPosition, 0];

        piecesOnSquares[endPosition, 1] = piecesOnSquares[startPosition, 1];

        piecesOnSquares[startPosition, 0] = Pieces.none;

        piecesOnSquares[startPosition, 1] = Pieces.noColor;
    }

    /*
     * loads the Positions of the pieces from a string to the array which stores the pieces
     */
    public void LoadPositionsFromString(string positions)
    {
        /*
         * splits the string to single characters
         */
        positions.Split();

        int rank = 0, file = 0;

        /*
         * cycles through every symbol of the string
         */
        foreach(char symbol in positions)
        {
            /*
             * checks if it should jump a rank of the board down
             */
            if(symbol == '/')
            {
                /*
                 * jumps a rank down
                 */
                rank++;
                file = 0;
            }
            else
            {
                /*
                 * checks if the symbol is a number
                 */
                if (char.IsDigit(symbol))
                {
                    /*
                     * sets the next squares to empty until the offset reaches the number of the current character
                     */
                    for (int offset = 0; offset < char.GetNumericValue(symbol); offset++)
                    {
                        piecesOnSquares[rank * 8 + file, 0] = Pieces.none;
                        piecesOnSquares[rank * 8 + file, 1] = Pieces.noColor;
                        file++;
                    } 
                }
                else
                {
                    /*
                     * translates the character to an integer using the dictionary and sets the piece-type to the result
                     */
                    int pieceType = getPieceTypeFromChar[char.ToLower(symbol)];

                    /*
                     * sets the color to white if the character is upppercase and sets it to black if the character is lowercase
                     */
                    int color = (char.IsUpper(symbol)) ? Pieces.white : Pieces.black;

                    /*
                     * changes the piece-type and color of the current square to the results
                     */
                    piecesOnSquares[rank * 8 + file, 0] = pieceType;
                    piecesOnSquares[rank * 8 + file, 1] = color;

                    file++;
                }
            }
        }

        /*
         * loads the prefabs of the pieces for every square
         */
        for(int r = 0; r < 8; r++)
        {
           for(int f = 0; f < 8; f++)
           {
                /*
                 * checks if there should be a piece
                 */
                if (piecesOnSquares[r * 8 + f, 0] != Pieces.none)
                {
                    /*
                     * gameobject that stores the loaded prefab
                     */
                    GameObject piece;

                    /*
                     * checks which color should be there
                     */
                    if (piecesOnSquares[r * 8 + f, 1] == Pieces.white)
                    {
                        /*
                         * loads the piece represented by the piece-value of the current square
                         */
                        switch (piecesOnSquares[r * 8 + f, 0])
                        {
                            case Pieces.pawn:
                                piece = Resources.Load("Prefabs/Pieces/pawnWhite") as GameObject;
                                break;
                            case Pieces.knight:
                                piece = Resources.Load("Prefabs/Pieces/nightWhite") as GameObject;
                                break;
                            case Pieces.bishop:
                                piece = Resources.Load("Prefabs/Pieces/bishopWhite") as GameObject;
                                break;
                            case Pieces.rook:
                                piece = Resources.Load("Prefabs/Pieces/rookWhite") as GameObject;
                                break;
                            case Pieces.king:
                                piece = Resources.Load("Prefabs/Pieces/kingWhite") as GameObject;
                                break;
                            default:
                                piece = Resources.Load("Prefabs/Pieces/queenWhite") as GameObject;
                                break;
                        }
                    }
                    else
                    {
                        /*
                        * loads the piece represented by the piece-value of the current square
                        */
                        switch (piecesOnSquares[r * 8 + f, 0])
                        {
                            case Pieces.pawn:
                                piece = Resources.Load("Prefabs/Pieces/pawnBlack") as GameObject;
                                break;
                            case Pieces.knight:
                                piece = Resources.Load("Prefabs/Pieces/nightBlack") as GameObject;
                                break;
                            case Pieces.bishop:
                                piece = Resources.Load("Prefabs/Pieces/bishopBlack") as GameObject;
                                break;
                            case Pieces.rook:
                                piece = Resources.Load("Prefabs/Pieces/rookBlack") as GameObject;
                                break;
                            case Pieces.king:
                                piece = Resources.Load("Prefabs/Pieces/kingBlack") as GameObject;
                                break;
                            default:
                                piece = Resources.Load("Prefabs/Pieces/queenBlack") as GameObject;
                                break;
                        }
                    }

                    /*
                     * changes the position of the piece to the position of the square on which it should be 
                     */
                    piece.transform.position = new Vector3(f - 3.5f, r - 3.5f, -1);

                    /*
                     * creates a new instance of the piece
                     */
                    Instantiate(piece);
                }
           }
        }
    }
}

/*
 * stores integer values of the piece-types and colors
 */
public static class Pieces
{
    public const int white = 1;
    public const int black = 0;
    public const int noColor = -1;

    public const int none = 0;
    public const int pawn = 1;
    public const int knight = 2;
    public const int bishop = 3;
    public const int rook = 4;
    public const int queen = 5;
    public const int king = 6;
}
