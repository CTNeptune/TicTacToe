using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuHandler : UIHandler, IUIHandler
{
    public GameObject _TitleObj;
    public TMP_Dropdown _SizeDropdown;
    public List<Vector2Int> _BoardSizeOptions;

    public TMP_InputField _InputField;
    public Button _StartBtn;
    
    private TicTacToeGameManager mGameManager;

    public void SetGameManager(TicTacToeGameManager inGameManager)
    {
        mGameManager = inGameManager;
    }

    private void Start()
    {
        List<string> optionStrings = new List<string>();

        foreach (var option in _BoardSizeOptions)
            optionStrings.Add(option.x + "x" + option.y);

        _SizeDropdown.ClearOptions();
        _SizeDropdown.AddOptions(optionStrings);
        _SizeDropdown.onValueChanged.AddListener(delegate { OnClickSizeButton(_SizeDropdown.value); });

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

    private void OnClickSizeButton(int inDropdownValue)
    {
        TrySetStartBtnActive();
    }

    private void TrySetStartBtnActive()
    {
        _StartBtn.interactable = !string.IsNullOrEmpty(_InputField.text);
    }

    private void OnClickStartGame()
    {
        try
        {
            mGameManager?.InitializeGame(_BoardSizeOptions[_SizeDropdown.value].x, _BoardSizeOptions[_SizeDropdown.value].y);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return;
        }

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
        gameObject.SetActive(inToggle);
    }
}