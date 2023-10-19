using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIPostGameMenuHandler : UIHandler, IUIHandler
{
    [SerializeField] private TextMeshProUGUI m_PostGameText;
    [SerializeField] private string m_WinnerTextString = "{0} won!";
    [SerializeField] private string m_DrawTextString = "Draw!";
    [SerializeField] Button m_EndGameButton;
    
    public void SetPlayerWinnerName(string inPlayerName)
    {
        m_PostGameText.text = string.Format(m_WinnerTextString, inPlayerName);
    }

    public void SetGameDrawText()
    {
        m_PostGameText.text = m_DrawTextString;
    }

    public void Show()
    {
        m_PostGameText.gameObject.SetActive(true);
        m_EndGameButton.gameObject.SetActive(true);
        m_EndGameButton.onClick.AddListener(() => OnEndGameButtonClicked());
    }

    private void OnEndGameButtonClicked()
    {
        Hide();
        UIManager.pInstance.ShowMainMenu();
    }

    public void Hide()
    {
        m_PostGameText.gameObject.SetActive(false);
        m_EndGameButton.gameObject.SetActive(false);
        m_EndGameButton.onClick.RemoveAllListeners();
    }
}
