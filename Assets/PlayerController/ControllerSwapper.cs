using UnityEngine;
using System.Collections;

public class ControllerSwapper : MonoBehaviour {

    public GameObject normalController;
    public GameObject gliderController;
    public GameObject lastController;
    public bool gliderMode = false;

	// Use this for initialization
	void Start ()
    {

	}

    void Update()
    {
        ChangeController();
    }
	
	// Update is called once per frame
	void ChangeController ()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Vector3 velocity = lastController.GetComponent<Rigidbody>().velocity;
            Vector3 position = lastController.transform.position;
            if (gliderMode)
            {
                Destroy(lastController);
                lastController = Instantiate(normalController, position, Quaternion.Euler(0, 0, 0)) as GameObject;
                lastController.GetComponent<Rigidbody>().velocity = velocity;
            }
            else
            {
                if (!lastController.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().Grounded)
                {
                    return;
                }
                Destroy(lastController);
                lastController = Instantiate(gliderController, position, Quaternion.Euler(0, 0, 0)) as GameObject;
                lastController.transform.LookAt(transform.position + velocity);
                lastController.GetComponent<Rigidbody>().velocity = velocity;
            }
        }	
	}
}
