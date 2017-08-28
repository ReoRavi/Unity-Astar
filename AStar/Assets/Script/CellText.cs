using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellText : MonoBehaviour {

    private Text text;

    public void SetText(int g, int f, int h)
    {
        if (text == null)
            text = GetComponent<Text>();

        text.text = " G : " + g.ToString() + "\n";
        text.text += " F : " + f.ToString() + "\n";
        text.text += " H : " + h.ToString() + "\n";
    }

    public void DeleteText()
    {
        if (text == null)
            text = GetComponent<Text>();

        text.text = "";
    }
}
