using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuHandler : UIHandler, IUIHandler
{
    [SerializeField] private TMP_Dropdown _PlayerCountDropdown;

    [SerializeField] private TMP_Dropdown _SizeDropdown;
    [SerializeField] private List<Vector2Int> _BoardSizeOptions;

    [SerializeField] private List<TMP_InputField> _PlayerInputFields;
    [SerializeField] private Button _StartBtn;
    
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

        _PlayerCountDropdown.onValueChanged.AddListener(delegate { OnClickPlayerCount(_PlayerCountDropdown.value); });

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
            foreach (TMP_InputField inputField in _PlayerInputFields)
                inputField.onValueChanged.RemoveAllListeners();

            _StartBtn.onClick.RemoveAllListeners();
            return;
        }

        foreach (TMP_InputField inputField in _PlayerInputFields)
            inputField.onValueChanged.AddListener(delegate { OnNameChanged(inputField, inputField.text); });

        _StartBtn.onClick.AddListener(() => OnClickStartGame());
    }

    private void OnNameChanged(TMP_InputField inInputfield, string text)
    {
        mGameManager.GetPlayers()[_PlayerInputFields.FindIndex(t => t == inInputfield)]._Name = text;
        TrySetStartBtnActive();
    }

    private void OnClickPlayerCount(int value)
    {
        for (int i = 0; i < _PlayerInputFields.Count; i++)
            _PlayerInputFields[i].transform.parent.gameObject.SetActive(false);

        for (int i = 0; i <= value; i++)
            _PlayerInputFields[i].transform.parent.gameObject.SetActive(true);

        List<Player> players = mGameManager.GetPlayers();
        players[1]._IsAI = value == 0;
        players[1]._Name = value > 0 ? "" : "AI";
        TrySetStartBtnActive();
    }

    private void OnClickSizeButton(int inDropdownValue)
    {
        TrySetStartBtnActive();
    }

    private void TrySetStartBtnActive()
    {
        _StartBtn.interactable = mGameManager.GetPlayers().Find(t => t._Name == "") == null;
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