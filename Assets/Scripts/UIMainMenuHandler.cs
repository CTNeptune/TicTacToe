using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuHandler : UIHandler, IUIHandler
{
    public GameObject _TitleObj;
    public List<BoardSizeButtons> _SizeBtns;
    private int mCurrentSize = -1;

    public TMP_InputField _InputField;
    public Button _StartBtn;
    
    private TicTacToeGameManager mGameManager;

    public void SetGameManager(TicTacToeGameManager inGameManager)
    {
        mGameManager = inGameManager;
    }

    private void Start()
    {
        for (int i = 0; i < _SizeBtns.Count; i++)
        {
            int index = i;
            _SizeBtns[i]._Button.onClick.AddListener(() => OnClickSizeButton(_SizeBtns[index]._Button, index));
        }

        SetListeners(true);
        _StartBtn.interactable = false;
    }

    private void SetListeners(bool inSet)
    {
        if (!inSet)
        {
            _InputField.onValueChanged.RemoveAllListeners();
            _StartBtn.onClick.RemoveAllListeners();
            return;
        }

        _InputField.onValueChanged.AddListener(delegate { OnNameChanged(_InputField.text); });
        _StartBtn.onClick.AddListener(() => OnClickStartGame());
    }

    private void OnNameChanged(string text)
    {
        mGameManager._Players[0]._Name = text;
        TrySetStartBtnActive();
    }

    private void OnClickSizeButton(Button inButton, int i)
    {
        inButton.Select();
        mCurrentSize = i;
        TrySetStartBtnActive();
    }

    private void TrySetStartBtnActive()
    {
        _StartBtn.interactable = !string.IsNullOrEmpty(_InputField.text) && mCurrentSize >= 0;
    }

    private void OnClickStartGame()
    {
        mGameManager?.InitializeGame(_SizeBtns[mCurrentSize]._Size, _SizeBtns[mCurrentSize]._Size);
        Hide();
    }

    public void Hide()
    {
        SetListeners(false);
        Toggle(false);
    }

    public void Show()
    {
        Toggle(true);
        SetListeners(true);
    }

    private void Toggle(bool inToggle)
    {
        _TitleObj.SetActive(inToggle);

        for (int i = 0; i < _SizeBtns.Count; i++)
            _SizeBtns[i]._Button.gameObject.SetActive(inToggle);

        _InputField.transform.parent.gameObject.SetActive(inToggle);
        _StartBtn.gameObject.SetActive(inToggle);
    }
}

[Serializable]
public class BoardSizeButtons
{
    public Button _Button;
    public int _Size; //Will have the same width and height, but we can add a custom width and height later if we want to
}