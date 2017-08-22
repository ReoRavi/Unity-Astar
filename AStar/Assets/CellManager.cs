using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour {

    // 셀 상태에 따라 바뀔 스프라이트
    public Sprite[] cellSprites;
    // 셀 목록
    private Cell[,] cells;
    // 열린 목록(최단거리일 가능성이 있는 셀들의 목록
    public List<Cell> openList;
    // 닫힌 목록(최단거리가 아닌 셀들의 목록)
    public List<Cell> closeList;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(int cellXCount, int cellYCount)
    {
        cells = new Cell[cellXCount, cellYCount];
        openList = new List<Cell>();
        closeList = new List<Cell>();
    }

    public void AddCell(Cell cell, int x, int y)
    {
        cells[x, y] = cell;
    }

    public void CalculateCellsHValue(int endCellX, int endCellY)
    {
        foreach (Cell cell in cells)
        {
            if (cell.endCell)
            {
                cell.h = 0;

                continue;
            }

            cell.h = (Mathf.Abs(cell.x - endCellX) + Mathf.Abs(cell.y - endCellY)) * 10;
        }
    }

    public void FindNextCenterCell(ref int centerCellXCount, ref int centerCellYCount)
    {
        if (openList.Count == 0)
            return;

        // Before CenterCell
        Cell beforeCell = cells[centerCellXCount, centerCellYCount];

        beforeCell.GetComponent<SpriteRenderer>().sprite = cellSprites[3];
        openList.Remove(beforeCell);
        closeList.Add(beforeCell);

        Cell nextCenterCell = cells[centerCellXCount - 1, centerCellYCount - 1];
        int centerX = 0;
        int centerY = 0;

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Cell cell = cells[centerCellXCount + x, centerCellYCount + y];

                if (cell.startCell)
                    continue;

                if (nextCenterCell.f >= cell.f)
                {
                    centerX = centerCellXCount + x;
                    centerY = centerCellYCount + y;

                    nextCenterCell = cell;
                }
            }
        }

        centerCellXCount = centerX;
        centerCellYCount = centerY;

        nextCenterCell.GetComponent<SpriteRenderer>().sprite = cellSprites[2];
    }

    public void CalculateRoundCellsGValue(ref int centerCellXCount, ref int centerCellYCount)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Cell cell = cells[centerCellXCount + x, centerCellYCount + y];

                if (cell.blockCell)
                    continue;

                if (closeList.Exists(e => e == cell))
                    continue;

                if (cell.x == centerCellXCount && cell.y == centerCellYCount)
                {
                    if (!(closeList.Exists(e => e == cell)))
                        closeList.Add(cell);

                    continue;
                }

                if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    cell.g = 14;
                else
                    cell.g = 10;

                //if (openList.Exists(e => e == cell))
                //{
                //    if (cell.g < cells[centerCellXCount, centerCellYCount].g + cell.g)
                //    {
                //        // 부모 셀이 바뀌고, 더 나은 주위 열린 노드에 더 나은 G 값이 있다면 옮김(대각선 값)
                //        Cell beforeCenterCell = cells[centerCellXCount, centerCellYCount];

                //        beforeCenterCell.GetComponent<SpriteRenderer>().sprite = cellSprites[3];
                //        openList.Remove(beforeCenterCell);
                //        closeList.Add(beforeCenterCell);

                //        centerCellXCount = centerCellXCount + x;
                //        centerCellYCount = centerCellYCount + y;

                //        cell.GetComponent<SpriteRenderer>().sprite = cellSprites[2];

                //        return;
                //    }
                //}
                //else
                    openList.Add(cell);

                cell.GetComponent<SpriteRenderer>().sprite = cellSprites[1];

                CalculateCellFValue(cell);
            }
        }

        RefreshOpenNodeText();
        //RefreshCloseNodeText();
        Debug.Log("GValue");
    }

    public void RefreshOpenNodeText()
    {
        foreach (Cell cell in openList)
        {
            cell.RefreshText();
        }
    }

    public void RefreshCloseNodeText()
    {
        foreach (Cell cell in closeList)
        {
            cell.DeleteText();
        }
    }

    public void CalculateCellFValue(Cell cell)
    {
        cell.f = cell.g + cell.h;
    }

}
