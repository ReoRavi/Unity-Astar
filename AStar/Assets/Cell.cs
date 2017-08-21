using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool startCell = false;
    public bool endCell = false;
    public bool blockCell = false;

    // Use this for initialization
    void Start () {
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

}
