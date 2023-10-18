using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGameManager : MonoBehaviour
{
    public List<Player> _Players = new List<Player>();
    private int mCurrentPlayerTurn;

    public TicTacToeGameBoard pBoard;

    private void Start()
    {
        StartCoroutine(WaitForUIManager());
    }

    private IEnumerator WaitForUIManager()
    {
        while (UIManager.pInstance == null)
            yield return new WaitForEndOfFrame();

        UIManager.pInstance.pTicTacToeGameManager = this;
    }

    public void InitializeGame(int boardWidth, int boardHeight)
    {
        pBoard.OnValidMove -= OnValidMove;
        pBoard.OnInvalidMove -= OnInvalidMove;
        pBoard.OnGameWon -= OnGameWon;

        pBoard.Initialize(boardWidth, boardHeight);

        pBoard.OnValidMove += OnValidMove;
        pBoard.OnInvalidMove += OnInvalidMove;
        pBoard.OnGameWon += OnGameWon;

        pBoard.SetCurrentPlayer(_Players[0]);
    }

    private void OnValidMove(Move inMove)
    {
        Debug.Log(inMove.Player._Name + " has made a valid move at " + inMove.MarkerX + " " + inMove.MarkerY);
        
        mCurrentPlayerTurn++;

        if (mCurrentPlayerTurn > _Players.Count - 1)
            mCurrentPlayerTurn = 0;
        
        pBoard.SetCurrentPlayer(_Players[mCurrentPlayerTurn]);
    }

    private void OnInvalidMove(Move inMove)
    {
        Debug.Log(inMove.Player._Name + " has attempted an invalid move at " + inMove.MarkerX + " " + inMove.MarkerY);
    }

    private void OnGameWon(Move lastMove)
    {
        pBoard.OnGameWon -= OnGameWon;
        Debug.Log(lastMove.Player._Name + " won!");
    }
}

[Serializable]
public class Player
{
    public string _Name;
    public bool _IsAI;
    public string _Symbol;
}

public class Move
{
    public Move(int markerX, int markerY, Player player)
    {
        MarkerX = markerX;
        MarkerY = markerY;
        Player = player;
    }

    public int MarkerX { get; }
    public int MarkerY { get; }
    public Player Player { get; }
}
