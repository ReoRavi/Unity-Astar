using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

public class CellManager : MonoBehaviour {

    // 셀 상태에 따라 바뀔 스프라이트
    public Sprite[] cellSprites;
    // 셀 목록
    public Cell[,] cells;
    // 열린 목록(최단거리일 가능성이 있는 셀들의 목록)
    public List<Cell> openList;
    // 닫힌 목록(최단거리가 아닌 셀들의 목록)
    public List<Cell> closeList;

    // 부모 셀
    public Cell parentCell;
    // 목표 셀
    public Cell targetCell;

    // 처음 시작인지
    public bool start;

    public void Init(int cellXCount, int cellYCount)
    {
        cells = new Cell[cellXCount, cellYCount];
        openList = new List<Cell>();
        closeList = new List<Cell>();

        start = false;
    }

    private void AddCloseCell(Cell cell)
    {
        closeList.Add(cell);
    }

    public void StartAStarPathfinding(int startCellXPos, int startCellYPos, int endCellXPos, int endCellYPos)
    {
        parentCell = cells[startCellXPos, startCellYPos];
        targetCell = cells[endCellXPos, endCellYPos];

        CalculateCellsHValue();

        // 시작 셀을 닫힌 목록에 추가
        AddCloseCell(parentCell);

        CalculateShortestDistance();

        start = true;
    }
   
    public bool CalculateShortestDistance()
    {
        Cell beforeCell = parentCell;

        if (start)
        {
            if (openList.Count == 0)
                Debug.Log("Can't Find");
            else
            {
                parentCell = GetShortestCellByFValue();

                // 닫힌 목록에 현재 부모를 넣는다.
                AddCloseCell(parentCell);
            }
        }

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (parentCell.x + x < 0 || parentCell.y + y < 0 ||
                    parentCell.x + x > AStarManager.Instance.xCellCount - 1 || 
                    parentCell.y + y > AStarManager.Instance.yCellCount - 1)
                    continue;

                if (parentCell == null || cells[parentCell.x + x, parentCell.y + y] == null)
                    Debug.Log("s");
                Cell cell = cells[parentCell.x + x, parentCell.y + y];

                if (cell.endCell)
                {
                    cell.parentCell = parentCell;

                    return true;
                }

                if (cell.blockCell)
                    continue;

                if (closeList.Exists(e => e == cell))
                    continue;

                // 부모 셀 주위 검색된 셀들을 오픈 리스트에 추가,
                // 이미 열린 목록에 존재한다면, 예비 부모 셀 -> 셀, 현재 부모 -> 셀 의 G값을 비교해서 최단거리 여부를 판단한다.
                if (openList.Find(e => e == cell))
                {
                    int beforeDistance = GetDistance(beforeCell, cell);
                    int currentDistance = parentCell.g + GetDistance(parentCell, cell);

                    // 이전 부모를 거쳐서 현재 부모로 가는 이동거리가 이전보다 길 경우
                    if (beforeDistance < currentDistance)
                    {
                        // 새로운 부모를 할당한다.
                        cell.parentCell = parentCell;
                    }
                }
                else
                {
                    openList.Add(cell);

                    cell.parentCell = parentCell;
                }

                // G값 세팅
                cell.SetCellGValue();

                // F값 세팅
                cell.SetCellFValue();
            }
        }

        //RefreshOpenNodeText();

        return false;
    }

    private int GetDistance(Cell currentCell, Cell targetCell)
    {
        int xAbs = Mathf.Abs(currentCell.x - targetCell.x);
        int yAbs = Mathf.Abs(currentCell.y - targetCell.y);
        if (xAbs == 1 && yAbs == 1)
            return 14;
        else
            return 10;
    }

    private Cell GetShortestCellByFValue()
    {
        Cell shortestCell = openList[0];

        foreach (Cell cell in openList)
        {
            if (cell.blockCell || closeList.Exists(e => e == cell))
                continue;

            if (cell.f <= shortestCell.f)
            {
                shortestCell = cell;
            }
        }

        return shortestCell;
    }

    public void CalculateCellsHValue()
    {
        foreach (Cell cell in cells)
        {
            if (cell.endCell)
            {
                cell.h = 0;

                continue;
            }

            cell.h = (Mathf.Abs(cell.x - targetCell.x) + Mathf.Abs(cell.y - targetCell.y)) * 10;
        }
    }

    //public void RefreshOpenNodeText()
    //{
    //    foreach (Cell cell in openList)
    //    {
    //        cell.RefreshText();
    //    }
    //}

    //public void RefreshCloseNodeText()
    //{
    //    foreach (Cell cell in closeList)
    //    {
    //        cell.DeleteText();
    //    }
    //}

    public void ShowAStarResult()
    {
        Cell cell = targetCell;

        while (true)
        {
            cell = cell.parentCell;

            if (cell.parentCell == null)
                break;
        }
    }

    #region Zergling

    public List<Cell> GetAStarPath(Cell currentCell, Cell targetCell)
    {
        foreach (Cell fCell in cells)
        {
            fCell.parentCell = null;
        }

        start = false;

        openList.Clear();
        closeList.Clear();

        parentCell = currentCell;
        this.targetCell = targetCell;

        CalculateCellsHValue();

        AddCloseCell(parentCell);

        while (true)
        {
            if (CalculateShortestDistance())
                break;

            if (!start)
                start = true;
        }

        List<Cell> path = new List<Cell>();

        Cell cell = targetCell;

        path.Add(cell);

        while (true)
        {
            if (cell.parentCell == null)
                break;

            cell = cell.parentCell;

            path.Add(cell);
        }

        foreach (Cell fCell in cells)
        {
            fCell.RefreshCell();
        }


        return path;
    }



    #endregion
}
