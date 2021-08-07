using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    /*
     * position where the piece was before it moved
     */
    private Vector3 oldPosition;

    /*
     * square where the piece is before it moved
     */
    private int oldSquare;

    /*
     * called when the player clicks on a piece
     */
    private void OnMouseDown()
    {
        /*
         * saves its current position to oldPosition
         */
        oldPosition = GetComponentInParent<Transform>().transform.position;

        /*
         * cycles through every square to get the square where the piece is
         */
        for (int pos = 0; pos < 64; pos++)
        {
            /*
             * the position of the current square
             */
            Vector3 squarePosition = Board.squares[pos].transform.position;

            /*
             * checks if the old position is within a range of +-.5f of the position of the current square
             */
            if (squarePosition.x - .5f <= oldPosition.x && squarePosition.x + .5f >= oldPosition.x && squarePosition.y - .5f <= oldPosition.y && squarePosition.y + .5 >= oldPosition.y)
            {
                /*
                 * sets the old square to the current square
                 */
                oldSquare = pos;

                break;
            }

        }
    }

    /*
     * called when the piece is dragged
     */
    void OnMouseDrag()
    {
        /*
         * gets the mouse-position at the screen and translates it to in-world-coordinates
         */
        Vector3 newPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);

        /*
         * changes the position of the piece to the mouse-position -> dragging a piece
         */
        GetComponentInParent<Transform>().transform.position = newPosition;
    }

    /*
     * called when the player releases the piece
     */
    void OnMouseUpAsButton()
    {
        Vector3 mousePosition, squarePosition;

        /*
         * gets the mouse-position at the screen and translates it to in-world-coordinates
         */
        mousePosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);

        /*
         * cycles through every square to get the square where the piece is
         */
        for (int pos = 0; pos < 64; pos++)
        {
            /*
             * gets the position of the current square
             */
            squarePosition = Board.squares[pos].transform.position;

            /*
             * checks if the square is empty
             */
            if (Board.piecesOnSquares[pos, 0] == 0)
            {
                /*
                 * checks if the mouse is over the current square
                 */
                if (squarePosition.x - .5f <= mousePosition.x && squarePosition.x + .5f >= mousePosition.x && squarePosition.y - .5f <= mousePosition.y && squarePosition.y + .5 >= mousePosition.y)
                {
                    squarePosition.z = -1;

                    /*
                     * sets the position of the piece to the position of the square -> moves it on the gui
                     */
                    GetComponentInParent<Transform>().transform.position = squarePosition;

                    /*
                     * moves the piece in the piece-array from its origin to its destination
                     * buggy af
                     */
                    //Board.movePieceOnSquare(oldSquare, pos);

                    /*
                     * breaks the for-loop so that the piece wont be moved back to its origin
                     */
                    break;
                }

            }
            else
            {
                /*
                 * moves the piece back to its old position
                 */
                GetComponentInParent<Transform>().transform.position = oldPosition;
            }
        }
    }
}
