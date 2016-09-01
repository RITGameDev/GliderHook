using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour
{

    public Transform cameraParent;
    public Transform lookAt;
    public Transform cameraLocation;
    public float sensetivity = 30;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        cameraParent.position = cameraLocation.position;
        Vector3 rotation = cameraLocation.rotation.eulerAngles;
        //rotation.x = 0;
        //rotation.z = 0;
        cameraParent.rotation = Quaternion.Euler(rotation);
        lookAt.RotateAround(cameraParent.position, cameraLocation.up, Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensetivity);
        lookAt.RotateAround(cameraParent.position, lookAt.right, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensetivity);
        transform.LookAt(lookAt, cameraLocation.up);
    }
}