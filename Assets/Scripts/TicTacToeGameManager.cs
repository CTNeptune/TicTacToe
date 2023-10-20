using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGameManager : MonoBehaviour
{
    [SerializeField] private List<Player> m_Players = new List<Player>();
    private int mCurrentPlayerTurn;

    public TicTacToeGameBoard pBoard;

    private void Start()
    {
        StartCoroutine(WaitForUIManager());
    }

    public List<Player> GetPlayers()
    {
        return m_Players;
    }

    private IEnumerator WaitForUIManager()
    {
        while (UIManager.pInstance == null)
            yield return new WaitForEndOfFrame();

        UIManager.pInstance.pTicTacToeGameManager = this;
    }

    public void InitializeGame(int boardWidth, int boardHeight)
    {
        SetDelegates(false);

        pBoard.Initialize(boardWidth, boardHeight);

        SetDelegates(true);

        mCurrentPlayerTurn = 0;
        pBoard.SetCurrentPlayer(m_Players[mCurrentPlayerTurn]);
        pBoard.SetBoardActive(true);
        pBoard.ToggleBoardGrid(true);
    }

    private void SetDelegates(bool Add)
    {
        if (Add)
        {
            pBoard.OnValidMove += OnValidMove;
            pBoard.OnInvalidMove += OnInvalidMove;
            pBoard.OnGameWon += OnGameWon;
            pBoard.OnGameDraw += OnGameDraw;
        }
        else
        {
            pBoard.OnValidMove -= OnValidMove;
            pBoard.OnInvalidMove -= OnInvalidMove;
            pBoard.OnGameWon -= OnGameWon;
            pBoard.OnGameDraw -= OnGameDraw;
        }
    }

    private void OnValidMove(Move inMove)
    {
        Debug.Log(inMove.Player._Name + " has made a valid move at " + inMove.MarkerX + " " + inMove.MarkerY);
        
        mCurrentPlayerTurn++;

        if (mCurrentPlayerTurn > m_Players.Count - 1)
            mCurrentPlayerTurn = 0;

        pBoard.SetCurrentPlayer(m_Players[mCurrentPlayerTurn]);
        
        if(m_Players[mCurrentPlayerTurn]._IsAI)
            m_Players[mCurrentPlayerTurn].AutoPerformMove(pBoard);
    }

    private void OnInvalidMove(Move inMove)
    {
        Debug.Log(inMove.Player._Name + " has attempted an invalid move at " + inMove.MarkerX + " " + inMove.MarkerY);
    }

    private void OnGameWon(Move winningMove)
    {
        SetDelegates(false);
        pBoard.SetBoardActive(false);
        UIManager.pInstance.ShowPostGameMenu(winningMove);
        Debug.Log(winningMove.Player._Name + " won!");
    }

    private void OnGameDraw(Move inMove)
    {
        SetDelegates(false);
        pBoard.SetBoardActive(false);
        UIManager.pInstance.ShowPostGameMenu(inMove, true);
        Debug.Log("Draw!");
    }

    public void ResetGameState()
    {
        pBoard.ResetBoard();
        pBoard.ToggleBoardGrid(false);
    }
}

[Serializable]
public class Player
{
    public string _Name;
    public bool _IsAI;
    public string _Symbol;

    public void AutoPerformMove(TicTacToeGameBoard inBoard)
    {
        Move randomMove = inBoard.CreateRandomMove();
        Move newMove = new Move(randomMove.MarkerX, randomMove.MarkerY, this);
        inBoard.PlaceMarker(newMove);
    }
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
