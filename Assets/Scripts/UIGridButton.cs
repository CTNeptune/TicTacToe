using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IMoveHandler
{
    void OnGridButtonClicked(UIGridButton inGridBtn);
    void PlaceMarker(Move inMove);
    bool IsEmptyCell(int x, int y);
    bool CheckWin(Move lastMove);
}

public class UIGridButton : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private TextMeshProUGUI m_SymbolTextField;

    private int mGridRow;
    private int mGridColumn;
    private IMoveHandler mMoveHandler;

    public void Initialize(int inGridRow, int inGridColumn, IMoveHandler inMoveHandler)
    {
        if (!m_Button)
        {
            Debug.LogError("No button assigned on " + gameObject.name);
            return;
        }

        m_Button.onClick.AddListener(OnButtonClick);
        mGridRow = inGridRow;
        mGridColumn = inGridColumn;
        mMoveHandler = inMoveHandler;
        gameObject.SetActive(true);
    }

    private void OnButtonClick()
    {
        mMoveHandler.OnGridButtonClicked(this);
    }

    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(mGridRow, mGridColumn);
    }

    public void SetSymbol(string inSymbol)
    {
        m_SymbolTextField.text = inSymbol;
    }
}
