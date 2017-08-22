using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour {

    // Cell Count
    public int xCellCount;
    public int yCellCount;

    // Start Cell(Green) Position
    public int startCellXPos;
    public int startCellYPos;

    // End Cell(Red) Position
    public int endCellXPos;
    public int endCellYPos;

    // Cell Object
    public GameObject cellPrefab;

    // Cell Manager
    private CellManager cellManager;

    // Use this for initialization
    void Start () {
        xCellCount = 14;
        yCellCount = 10;
        
        startCellXPos = 3;
        startCellYPos = 3;

        endCellXPos = 10;
        endCellYPos = 3;

        cellManager = GetComponent<CellManager>();
        cellManager.Init(xCellCount, yCellCount);

        for (int x = 0; x < xCellCount; x++)
        {
            for (int y = 0; y < yCellCount; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, new Vector3(-6.15F + x, 4.5F - y, 0), Quaternion.identity);

                Cell cell = cellObject.GetComponent<Cell>();

                if (x == startCellXPos && y == startCellYPos)
                {
                    cell.startCell = true;
                    cellObject.GetComponent<SpriteRenderer>().color = Color.green;
                }

                if (x == endCellXPos && y == endCellYPos)
                {
                    cell.endCell = true;
                    cellObject.GetComponent<SpriteRenderer>().color = Color.red;
                }

                cell.x = x;
                cell.y = y;

                cellManager.AddCell(cell, x, y);
            }
        }

        cellManager.CalculateCellsHValue(endCellXPos, endCellYPos);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cellManager.FindNextCenterCell(ref startCellXPos, ref startCellYPos);
            cellManager.CalculateRoundCellsGValue(ref startCellXPos, ref startCellYPos);
        }
	}
}
