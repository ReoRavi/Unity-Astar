using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum eState { eWait, eSearching, eFinish, eNone }

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

    // State
    private eState state;

    // Use this for initialization
    void Start () {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        xCellCount = (int)(screenSize.x * 2 / cellPrefab.transform.localScale.x);
        yCellCount = (int)(screenSize.y * 2 / cellPrefab.transform.localScale.y);
        
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
                GameObject cellObject = Instantiate(cellPrefab, new Vector3(
                    -screenSize.x + (x * cellPrefab.transform.localScale.x) + (cellPrefab.transform.localScale.x / 2), 
                    screenSize.y - (y * cellPrefab.transform.localScale.y) - (cellPrefab.transform.localScale.y / 2), 0), 
                    Quaternion.identity);

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

                cellManager.cells[x, y] = cell;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switch (state)
            {
                case eState.eWait:
                    cellManager.StartAStarPathfinding(startCellXPos, startCellYPos, endCellXPos, endCellYPos);
                    state = eState.eSearching;

                    break;

                case eState.eSearching:
                    if (cellManager.CalculateShortestDistance())
                        state = eState.eFinish;

                    break;
                case eState.eFinish:
                    cellManager.ShowAStarResult();
                    state = eState.eNone;

                    break;
            }
        }
	}
}
