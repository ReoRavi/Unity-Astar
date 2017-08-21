using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour {

    // Cell Count
    public int xCellCount;
    public int yCellCount;

    // Start Cell(Green) Position
    public float startCellXPos;
    public float startCellYPos;

    // End Cell(Red) Position
    public float endCellXPos;
    public float endCellYPos;

    // Cell Object
    public GameObject cellPrefab;

    // Use this for initialization
    void Start () {
        xCellCount = 14;
        yCellCount = 10;

        startCellXPos = 3;
        startCellYPos = 3;

        endCellXPos = 10;
        endCellYPos = 3;

        for (int x = 0; x < xCellCount; x++)
        {
            for (int y = 0; y < yCellCount; y++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(-6.15F + x, 4.5F - y, 0), Quaternion.identity);

                if (x == startCellXPos && y == startCellYPos)
                    cell.GetComponent<SpriteRenderer>().color = Color.green;

                if (x == endCellXPos && y == endCellYPos)
                    cell.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
