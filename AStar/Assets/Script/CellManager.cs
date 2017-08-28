using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    #region Variable
    // First Start - Required Init()
    public bool start;

    [Header("Cell Sprites")]
    [SerializeField]
    // Cell Sprites
    private Sprite[] cellSprites;

    // Cell Objects
    private Cell[,] cells;
    // Open List (Possible Shortest Distance)
    private List<Cell> openList;
    // Close List (Not Shortest Distance Cell)
    private List<Cell> closeList;

    // Parent Cell
    private Cell parentCell;
    // Target Cell
    private Cell targetCell;

    // Max Cell Count
    private int xCellCount, yCellCount;
    #endregion

    #region Initalize
    // Initialize Variable
    public void Init(int xCellCount, int yCellCount)
    {
        cells = new Cell[xCellCount, yCellCount];
        openList = new List<Cell>();
        closeList = new List<Cell>();

        this.xCellCount = xCellCount;
        this.yCellCount = yCellCount;

        start = false;
    }

    // Start Find Path to Step by Step
    public void StartAStarPathfinding(int startCellXPos, int startCellYPos, int endCellXPos, int endCellYPos)
    {
        parentCell = cells[startCellXPos, startCellYPos];
        targetCell = cells[endCellXPos, endCellYPos];

        SetCellsHValue();

        closeList.Add(parentCell);

        AStarPathFinding();

        start = true;
    }
    #endregion

    #region PathFinding
    // Do PathFinding
    // If Find Shortest Way, Function will return True
    public bool AStarPathFinding()
    {
        Cell beforeCell = parentCell;

        if (start)
        {
            if (openList.Count == 0)
                Debug.Log("Can't Find");
            else
            {
                parentCell = GetShortestCellByFValue();

                closeList.Add(parentCell);
            }
        }

        // Search round cells
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                int cellX = parentCell.GetCellX();
                int cellY = parentCell.GetCellY();

                if (cellX + x < 0 ||
                    cellY + y < 0 ||
                    cellX + x > xCellCount - 1 ||
                    cellY + y > yCellCount - 1)
                    continue;

                Cell cell = cells[cellX + x, cellY + y];

                eCellState cellState = cell.GetCellState();

                if (cellState == eCellState.eEnd)
                {
                    cell.parentCell = parentCell;

                    return true;
                }

                if (cellState == eCellState.eBlock)
                    continue;

                if (closeList.Exists(e => e == cell))
                    continue;

                // If Exist in OpenList, Check Not Shortest Way and Set New Parent Cell
                if (openList.Find(e => e == cell))
                {
                    // Distance by Before Cell
                    int beforeDistance = GetDistance(beforeCell, cell);
                    // Distance by Current Cell
                    int currentDistance = parentCell.GetCellG() + GetDistance(parentCell, cell);

                    // Current Distance is not Shortest Way
                    if (beforeDistance < currentDistance)
                    {
                        cell.parentCell = parentCell;
                    }
                }
                // Add Round Cell to Open List
                else
                {
                    openList.Add(cell);

                    cell.parentCell = parentCell;
                }

                // Reset Cell Value
                cell.SetGValue();
                cell.SetFValue();

                cell.GetComponent<SpriteRenderer>().sprite = cellSprites[1];
            }
        }

        RefreshOpenNodeText();

        parentCell.GetComponent<SpriteRenderer>().sprite = cellSprites[2];

        return false;
    }

    // Get Distance CurrentCell to TargetCell
    // Diagonal Angle - return 14, else return 10
    private int GetDistance(Cell currentCell, Cell targetCell)
    {
        int xAbs = Mathf.Abs(currentCell.GetCellX() - targetCell.GetCellX());
        int yAbs = Mathf.Abs(currentCell.GetCellY() - targetCell.GetCellY());
        if (xAbs == 1 && yAbs == 1)
            return 14;
        else
            return 10;
    }

    // Get Shortest Cell in Round Cell
    private Cell GetShortestCellByFValue()
    {
        Cell shortestCell = openList[0];

        foreach (Cell cell in openList)
        {
            if (cell.GetCellState() == eCellState.eBlock || closeList.Exists(e => e == cell))
                continue;

            if (cell.GetCellF() <= shortestCell.GetCellF())
            {
                shortestCell = cell;
            }
        }

        shortestCell.GetComponent<SpriteRenderer>().sprite = cellSprites[2];

        return shortestCell;
    }

    // Set Cell
    public void SetCell(Cell cell, int x, int y)
    {
        cells[x, y] = cell;
    }

    // Set Cells H Value
    public void SetCellsHValue()
    {
        foreach (Cell cell in cells)
        {
            if (cell.GetCellState() == eCellState.eEnd)
            {
                cell.SetHValue(0);

                continue;
            }

            cell.SetHValue((Mathf.Abs(cell.GetCellX() - targetCell.GetCellX()) + Mathf.Abs(cell.GetCellY() - targetCell.GetCellY())) * 10);
        }
    }

    // Refresh Node Text
    public void RefreshOpenNodeText()
    {
        foreach (Cell cell in openList)
        {
            cell.RefreshText();
        }
    }

    // Show Path, Change Shortest Cells Sprite
    public void ShowAStarResult()
    {
        Cell cell = targetCell;

        while (true)
        {
            cell = cell.parentCell;
            cell.GetComponent<SpriteRenderer>().sprite = cellSprites[3];

            if (cell.parentCell == null)
                break;
        }
    }
    #endregion
}
