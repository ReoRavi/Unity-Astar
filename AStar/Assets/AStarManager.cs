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

    // zergling
    public GameObject zergling;
    // zergling Prefab
    public List<GameObject> zerglings;
    
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

        Vector3 cellScale = cellPrefab.transform.localScale;

        for (int x = 0; x < xCellCount; x++)
        {
            for (int y = 0; y < yCellCount; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, new Vector3(
                    -screenSize.x + (x * cellScale.x) + (cellScale.x / 2), 
                    screenSize.y - (y * cellScale.y) - (cellScale.y / 2), 0), 
                    Quaternion.identity);

                Cell cell = cellObject.GetComponent<Cell>();

                if (x == startCellXPos && y == startCellYPos)
                {
                    cell.startCell = true;
                }

                if (x == endCellXPos && y == endCellYPos)
                {
                    cell.endCell = true;
                }

                cell.x = x;
                cell.y = y;

                cellManager.cells[x, y] = cell;
            }
        }

        for (int d = 0; d < 1; d++)
        {
            for (int y = 0; y < 1; y++)
            {
                zerglings.Add(Instantiate(zergling, new Vector3(6 - (0.4F * d), 4 - (0.4F * y), 0), Quaternion.identity));
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            cellManager.StartAStarPathfinding(startCellXPos, startCellYPos, endCellXPos, endCellYPos);

            while (true)
            {
                if (cellManager.CalculateShortestDistance())
                    break;
            }

            cellManager.ShowAStarResult();
        }
    }

    public List<Cell> GetAStarPath(Cell currentCell, Cell parentCell)
    {
        return cellManager.GetAStarPath(currentCell, parentCell);
    }

    public void ZerglingMove(Cell target, Vector3 touchPos)
    {
        foreach (GameObject zerg in zerglings)
        {
            zerg.GetComponent<Zergling>().Move(target, touchPos);
        }

        target.endCell = false;
    }

    private static AStarManager _instance;
    public static AStarManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(AStarManager)) as AStarManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "AStarManager";
                    _instance = container.AddComponent(typeof(AStarManager)) as AStarManager;
                }
            }

            return _instance;
        }
    }
}
