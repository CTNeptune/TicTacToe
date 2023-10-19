using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGameBoard : MonoBehaviour, IMoveHandler
{
    public UIGridButton _UIGridButtonTemplate;
    public GridLayoutGroup _GridLayout;
    private List<Move> pBoardState;

    private int pBoardWidth;
    private int pBoardHeight;

    public Action<Move> OnGameWon;
    public Action<Move> OnValidMove;
    public Action<Move> OnInvalidMove;

    private Player pCurrentPlayer;

    private List<UIGridButton> mGridButtonPool = new List<UIGridButton>();

    public void Initialize(int width, int height)
    {
        _GridLayout.constraintCount = width;
        pBoardWidth = width;
        pBoardHeight = height;
        pBoardState = new List<Move>();
        PopulateGrid();
    }

    private void PopulateGrid()
    {
        int requiredGridButtons = pBoardWidth * pBoardHeight;
        int currentGridButtonCount = mGridButtonPool.Count;

        foreach (var gridbutton in mGridButtonPool)
            gridbutton.gameObject.SetActive(false);

        if (currentGridButtonCount < requiredGridButtons)
        {
            for (int i = 0; i < requiredGridButtons - currentGridButtonCount; i++)
            {
                int gridRow = i / pBoardWidth;
                int gridColumn = i % pBoardWidth;
                UIGridButton newButton = Instantiate(_UIGridButtonTemplate, _UIGridButtonTemplate.transform.parent);
                newButton.Initialize(gridRow, gridColumn, this);
                mGridButtonPool.Add(newButton);
            }
            return;
        }

        for (int i = 0; i < requiredGridButtons; i++)
        {
            int gridRow = i / pBoardWidth;
            int gridColumn = i % pBoardWidth;

            mGridButtonPool[i].Initialize(gridRow, gridColumn, this);
        }
    }

    public void SetCurrentPlayer(Player inPlayer)
    {
        pCurrentPlayer = inPlayer;
    }

    public bool IsEmptyCell(int inX, int inY)
    {
        return pBoardState.Find(t => t.MarkerX == inX && t.MarkerY == inY) == null;
    }

    public Move CreateRandomMove()
    {
        List<Move> potentialMoves = new List<Move>();

        for (int i = 0; i < pBoardWidth; i++)
        {
            for (int j = 0; j < pBoardHeight; j++)
            {
                if (IsEmptyCell(i, j))
                    potentialMoves.Add(new Move(i, j, null));
            }
        }

        if (potentialMoves.Count == 0)
            return null;

        return potentialMoves[UnityEngine.Random.Range(0, potentialMoves.Count - 1)];
    }

    public void OnGridButtonClicked(UIGridButton inGridBtn)
    {
        Vector2Int gridBtnPos = inGridBtn.GetGridPosition();
        Move newMove = new Move(gridBtnPos.x, gridBtnPos.y, pCurrentPlayer);
        PlaceMarker(newMove);
    }

    public void PlaceMarker(Move inMove)
    {
        if (!IsEmptyCell(inMove.MarkerX, inMove.MarkerY) || inMove.MarkerX < 0 || inMove.MarkerX > pBoardWidth || inMove.MarkerY < 0 || inMove.MarkerY > pBoardHeight)
        {
            OnInvalidMove(inMove);
            return;
        }

        pBoardState.Add(inMove);
        UpdateGridBtnSymbol(pCurrentPlayer, inMove.MarkerX, inMove.MarkerY);
        CheckWin(inMove);
    }

    private void UpdateGridBtnSymbol(Player pCurrentPlayer, int markerX, int markerY)
    {
        UIGridButton btn = mGridButtonPool.Find(t => t.GetGridPosition() == new Vector2(markerX, markerY));
        btn.SetSymbol(pCurrentPlayer._Symbol);
    }

    public bool CheckWin(Move inLastMove)
    {
        List<Move> playerMoves = pBoardState.FindAll(t => t.Player == inLastMove.Player);

        if (playerMoves.Count == 0)
            return false;

        bool gameWon = HorizontalWin(playerMoves) || VerticalWin(playerMoves) || DiagonalWin(playerMoves);

        if (gameWon)
            OnGameWon.Invoke(inLastMove);
        else
            OnValidMove(inLastMove);

        return gameWon;
    }

    private bool HorizontalWin(List<Move> playerMoves)
    {
        for (int i = 0; i < playerMoves.Count; i++)
        {
            Move MoveLeft = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX - 1 && t.MarkerY == playerMoves[i].MarkerY);
            Move MoveRight = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX + 1 && t.MarkerY == playerMoves[i].MarkerY);

            if (MoveLeft != null && MoveRight != null)
                return true;
        }

        return false;
    }

    private bool VerticalWin(List<Move> playerMoves)
    {
        for (int i = 0; i < playerMoves.Count; i++)
        {
            Move MoveAbove = playerMoves.Find(t => t.MarkerY == playerMoves[i].MarkerY - 1 && t.MarkerX == playerMoves[i].MarkerX);
            Move MoveBelow = playerMoves.Find(t => t.MarkerY == playerMoves[i].MarkerY + 1 && t.MarkerX == playerMoves[i].MarkerX);

            if (MoveAbove != null && MoveBelow != null)
                return true;
        }

        return false;
    }

    private bool DiagonalWin(List<Move> playerMoves)
    {
        for (int i = 0; i < playerMoves.Count; i++)
        {
            Move MoveTopLeft = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX - 1 && t.MarkerY == playerMoves[i].MarkerY - 1);
            Move MoveBottomRight = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX + 1 && t.MarkerY == playerMoves[i].MarkerY + 1);

            if (MoveTopLeft != null && MoveBottomRight != null)
                return true;

            Move MoveTopRight = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX + 1 && t.MarkerY == playerMoves[i].MarkerY - 1);
            Move MoveBottomLeft = playerMoves.Find(t => t.MarkerX == playerMoves[i].MarkerX - 1 && t.MarkerY == playerMoves[i].MarkerY + 1);

            if (MoveTopRight != null && MoveBottomLeft != null)
                return true;
        }

        return false;
    }
}
