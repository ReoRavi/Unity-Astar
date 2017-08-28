using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCellState { eStart, eEnd, eBlock, eNone }

public class Cell : MonoBehaviour
{
    #region Variable
    // Parent Cell
    public Cell parentCell;

    // Cell State
    [Header("Cell State")]
    [SerializeField]
    private eCellState state;

    // Cell Position
    [Header("Cell Position")]
    [SerializeField]
    private int x, y;

    // G + H 
    [SerializeField]
    private int f;
    // Move Cost To Parent
    [SerializeField]
    private int g;
    // Direct Move Cost To EndCell 
    [SerializeField]
    private int h;

    // Cell Text Prefab
    [Header("Cell Text Prefab")]
    [SerializeField]
    private GameObject cellText;

    // Text Object
    private CellText cellTextObject;
    // Text Canvas
    private GameObject cellTextParentObject;
    #endregion

    #region Initailze
    // Use this for initialization
    void Awake()
    {
        state = eCellState.eNone;

        f = 0;
        g = 0;
        h = 0;

        parentCell = null;

        cellTextParentObject = Instantiate(cellText, transform.position, Quaternion.identity);
        cellTextParentObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    #endregion

    #region Cell
    // Get Cell State
    public eCellState GetCellState()
    {
        return state;
    }

    // Get Cell X Position
    public int GetCellX()
    {
        return x;
    }

    // Get Cell Y Position
    public int GetCellY()
    {
        return y;
    }

    // Get Cell F Value
    public int GetCellF()
    {
        return f;
    }

    // Get Cell G Value
    public int GetCellG()
    {
        return g;
    }

    // Get Cell H Value
    public int GetCellH()
    {
        return h;
    }

    public void SetCellState(eCellState state)
    {
        this.state = state;
    }

    // Set Cell Position
    public void SetCellPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Set H Value
    public void SetHValue(int h)
    {
        this.h = h;
    }

    // Calculate G Value
    public void SetGValue()
    {
        if (Mathf.Abs(parentCell.x - x) == 1 && Mathf.Abs(parentCell.y - y) == 1)
            g = 14;
        else
            g = 10;

    }

    // Calculate F Value
    public void SetFValue()
    {
        f = g + h;
    }

    // Mouse Down
    private void OnMouseDown()
    {
        if (state == eCellState.eStart ||
            state == eCellState.eEnd)
            return;

        if (state == eCellState.eBlock)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            state = eCellState.eNone;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            state = eCellState.eBlock;
        }
    }
    #endregion

    #region Text
    // Refresh Text
    public void RefreshText()
    {
        if (cellTextObject == null)
            cellTextObject = cellTextParentObject.transform.GetChild(0).GetComponent<CellText>();

        cellTextObject.GetComponent<CellText>().SetText(g, f, h);
        cellTextObject.transform.position = transform.position;
    }

    // Delete Text
    public void DeleteText()
    {
        if (cellTextObject == null)
            cellTextObject = cellTextParentObject.transform.GetChild(0).GetComponent<CellText>();

        cellTextObject.GetComponent<CellText>().DeleteText();
    }
    #endregion
}
