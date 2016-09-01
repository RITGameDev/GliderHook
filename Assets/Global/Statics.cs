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
}
