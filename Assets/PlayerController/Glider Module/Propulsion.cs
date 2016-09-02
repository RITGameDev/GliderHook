using UnityEngine;
using System.Collections;

public class Propulsion : MonoBehaviour {

    public float forwardForce = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * forwardForce * Time.fixedDeltaTime);
	}
}
