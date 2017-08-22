﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool startCell;
    public bool endCell;
    public bool blockCell;

    // Cell Position
    public int x;
    public int y;

    // G + H 
    public int f;
    // 현재 위치에서 이동 값(가로, 세로 = 10, 대각선 = 14)
    public int g;
    // 벽을 계산하지 않고 목적지에 도착하는 비용
    public int h;

    // 셀의 부모 (셀의 시작점)
    public Cell parentCell;

    // Cell Text
    public GameObject cellText;

    // 셀 텍스트 객체
    private CellText cellTextObject;
    // 부모 캔버스
    GameObject cellTextParentObject;

    // Use this for initialization
    void Awake () {
        startCell = false;
        endCell = false;
        blockCell = false;

        f = 0;
        g = 0;
        h = 0;

        parentCell = null;

        cellTextParentObject = Instantiate(cellText, transform.position, Quaternion.identity);
        cellTextParentObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (startCell || endCell)
            return;

        if (blockCell)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            blockCell = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            blockCell = true;
        }
    }

    // 셀의 G값을 찾는다.
    public void SetCellGValue(int x, int y)
    {
        if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
            g = 14;
        else
            g = 10;

    }

    // 셀의 F값을 찾는다.
    public void SetCellFValue()
    {
        f = g + h;
    }

    public void RefreshText()
    {
        if (cellTextObject == null)
            cellTextObject = cellTextParentObject.transform.GetChild(0).GetComponent<CellText>();

        cellTextObject.GetComponent<CellText>().SetText(g, f, h);
        cellTextObject.transform.position = transform.position;
    }

    public void DeleteText()
    {
        if (cellTextObject == null)
            cellTextObject = cellTextParentObject.transform.GetChild(0).GetComponent<CellText>();

        cellTextObject.GetComponent<CellText>().DeleteText();
    }
}
