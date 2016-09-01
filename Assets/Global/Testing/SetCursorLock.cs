using UnityEngine;
using System.Collections;

public class SetCursorLock : MonoBehaviour {

    public bool setLockTo = true;

	void Start () {
        Statics.SetCursorLock(setLockTo);
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Statics.SetCursorLock(false);
        }
        if (Input.GetMouseButton(0))
        {
            Statics.SetCursorLock(true);
        }
    }
}
