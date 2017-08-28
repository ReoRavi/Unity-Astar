using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum eAStarState { eWait, eSearching, eFinish, eNone }

public class AStarManager : MonoBehaviour
{
    #region Variable
    // Start Cell(Green) Position
    [Header("Start Cell Position")]
    [SerializeField]
    private int startCellXPos, startCellYPos;

    // End Cell(Red) Position
    [Header("End Cell Position")]
    [SerializeField]
    private int endCellXPos, endCellYPos;

    [Header("Cell Prefab Object")]
    [SerializeField]
    // Cell Object
    private GameObject cellPrefab;

    // Cell Manager
    private CellManager cellManager;
    // State
    private eAStarState state;
    #endregion

    // Use this for initialization
    void Start()
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        int xCellCount = (int)(screenSize.x * 2 / cellPrefab.transform.localScale.x);
        int yCellCount = (int)(screenSize.y * 2 / cellPrefab.transform.localScale.y);

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
                    cell.SetCellState(eCellState.eStart);
                    cellObject.GetComponent<SpriteRenderer>().color = Color.green;
                }

                if (x == endCellXPos && y == endCellYPos)
                {
                    cell.SetCellState(eCellState.eEnd);
                    cellObject.GetComponent<SpriteRenderer>().color = Color.red;
                }

                cell.SetCellPosition(x, y);
                cellManager.SetCell(cell, x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switch (state)
            {
                case eAStarState.eWait:
                    cellManager.StartAStarPathfinding(startCellXPos, startCellYPos, endCellXPos, endCellYPos);
                    state = eAStarState.eSearching;

                    break;

                case eAStarState.eSearching:
                    if (cellManager.AStarPathFinding())
                        state = eAStarState.eFinish;

                    break;
                case eAStarState.eFinish:
                    cellManager.ShowAStarResult();
                    state = eAStarState.eNone;

                    break;
            }
        }
    }
}
