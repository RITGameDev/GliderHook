using UnityEngine;
using System.Collections;

public class ControllerSwapper : MonoBehaviour {

    public GameObject normalController;
    public GameObject gliderController;
    public GameObject lastController;
    public ThirdPersonCamera tpCamera;
    public bool gliderMode = false;

	// Use this for initialization
	void Start ()
    {

	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeController();
        }
    }

    void OnGUI()
    {
        Statics.DisplayInfo("Speed: " + (lastController.GetComponent<Rigidbody>().velocity.magnitude * 3.6f).ToString("0.0") + " km/h",1);
    }

    // Update is called once per frame
    void ChangeController()
    {
        Debug.Log("Tab Pressed");
        Vector3 velocity = lastController.GetComponent<Rigidbody>().velocity;
        Vector3 position = lastController.transform.position;
        if (gliderMode)
        {
            Destroy(lastController);
            lastController = Instantiate(normalController, position, Quaternion.Euler(0, 0, 0)) as GameObject;
            lastController.GetComponent<Rigidbody>().velocity = velocity;
            lastController.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().cam = tpCamera.GetComponent<Camera>();
            gliderMode = false;
        }
        else
        {
            if (lastController.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().Grounded)
            {
                Debug.Log("Can't switch on the ground");
                return;
            }
            Destroy(lastController);
            lastController = Instantiate(gliderController, position, Quaternion.Euler(0, 0, 0)) as GameObject;
            lastController.transform.LookAt(lastController.transform.position + velocity);
            lastController.GetComponent<Rigidbody>().velocity = velocity;
            lastController.GetComponent<Glider>().cameraTransform = tpCamera.transform;
            gliderMode = true;
        }
        tpCamera.target = lastController.transform;
    }
}
