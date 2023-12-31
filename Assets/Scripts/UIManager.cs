using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager pInstance { get; private set; }

    public List<UIHandler> _UIHandlers = new List<UIHandler>();

    public TicTacToeGameManager pTicTacToeGameManager { get => mTicTacToeGameManager; set => mTicTacToeGameManager = value; }
    private TicTacToeGameManager mTicTacToeGameManager;

    private void Awake()
    {
        if (pInstance == null)
        {
            pInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        HideAllUI();
        StartCoroutine(WaitForInitialization());
    }

    IEnumerator WaitForInitialization()
    {
        while (mTicTacToeGameManager == null)
            yield return new WaitForEndOfFrame();

        SetupMainMenuHandlers();
        ShowMainMenu();
    }

    private void SetupMainMenuHandlers()
    {
        foreach (var handler in _UIHandlers)
        {
            if (handler is UIMainMenuHandler mainMenuHandler)
                mainMenuHandler.SetGameManager(mTicTacToeGameManager);
        }
    }

    public void ShowMainMenu()
    {
        mTicTacToeGameManager.ResetGameState();
        HideAllUI();
        (_UIHandlers.Find(t => t is UIMainMenuHandler) as UIMainMenuHandler)?.Show();
    }

    public void ShowInGameHUD()
    {
        HideAllUI();
    }

    public void ShowPostGameMenu(Move winningMove, bool isDraw = false)
    {
        HideAllUI();
        
        UIPostGameMenuHandler postGameMenuHandler = (_UIHandlers.Find(t => t is UIPostGameMenuHandler) as UIPostGameMenuHandler);
        
        if (postGameMenuHandler == null)
            return;

        if (isDraw)
            postGameMenuHandler.SetGameDrawText();
        else
            postGameMenuHandler.SetPlayerWinnerName(winningMove.Player._Name);
        
        postGameMenuHandler.Show();
    }

    public void HideAllUI()
    {
        foreach (IUIHandler handler in _UIHandlers)
            handler.Hide();
    }
}
