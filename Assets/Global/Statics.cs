using UnityEngine;
using System.Collections;

public delegate float FloatyInput(float sensetivity);
public delegate bool BoolControl();

public static class Statics {

	public static void SetCursorLock(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void DisplayInfo(string info,int area)
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(10, area * 30, 200, 20), info);
    }
}
