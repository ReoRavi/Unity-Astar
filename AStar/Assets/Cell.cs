using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    // Is StartCell
    public bool startCell;
    // Is EndCell
    public bool endCell;
    // Is BlockCell
    public bool blockCell;

    // Cell Position
    public int x;
    public int y;

    // G + H 
    public int f;
    // Move Price To Parent
    public int g;
    // Direct Price To EndCell 
    public int h;

    // Parent Cell
    public Cell parentCell;

#if TEXT
    // Cell Text Prefab
    public GameObject cellText;
    // Text Object
    private CellText cellTextObject;
    // Text Canvas
    GameObject cellTextParentObject;
#endif

    // Use this for initialization
    void Awake () {
        startCell = false;
        endCell = false;
        blockCell = false;

        f = 0;
        g = 0;
        h = 0;

        x = 0;
        y = 0;

        parentCell = null;


#if TEXT
        cellTextParentObject = Instantiate(cellText, transform.position, Quaternion.identity);
        cellTextParentObject.GetComponent<Canvas>().worldCamera = Camera.main;
#endif
    }

    public void RefreshCell()
    {
        f = 0;
        g = 0;
        h = 0;

        startCell = false;
        //endCell = false;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.transform == transform)
            {
                endCell = true;
                AStarManager.Instance.ZerglingMove(this, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    private void OnMouseDown()
    {
        if (startCell || endCell)
            return;

        if (blockCell)
        {
            blockCell = false;
        }
        else
        {
            blockCell = true;
        }
    }

    // Calculate G Value
    public void SetCellGValue()
    {
        if (Mathf.Abs(parentCell.x - x) == 1 && Mathf.Abs(parentCell.y - y) == 1)
            g = 14;
        else
            g = 10;

    }

    // Calculate F Value
    public void SetCellFValue()
    {
        f = g + h;
    }
#if TEXT
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
#endif
}
