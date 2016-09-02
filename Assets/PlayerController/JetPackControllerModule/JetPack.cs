using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController))]
public class JetPack : MonoBehaviour {

    public float upForce = 10f;
    public float startingFuel = 10f;
    public float maxFuel = 10f;
    public float regenerationRate = .5f;
    public float burnRate = 2f;
    public float zBurnRate = 1f;
    public float zForce = 100f;
    public float xBurnRate = 1f;
    public float xForce = 100f;
    public float activatedDragDelta = 3f;

    protected FloatyInput xInput;
    protected FloatyInput zInput;
    protected BoolControl activeInput;

    public float fuel;
    protected UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controller;
    protected Rigidbody rb;
    protected float lastDragDelta = 0f;
	// Use this for initialization
	void Start ()
    {
        fuel = startingFuel;
        controller = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
        SetupInputDelegates();
	}

    #region SetupInputs
    void SetupInputDelegates()
    {
        if (xInput == null)
        {
            xInput = GetXInput;
        }
        if (zInput == null)
        {
            zInput = GetZInput;
        }
        if(activeInput == null)
        {
            activeInput = GetActiveationInput;
        }
    }

    protected float GetXInput(float sensetivity)
    {
        float rVal = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rVal -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rVal += 1f;
        }
        return rVal * sensetivity;
    }

    protected float GetZInput(float sensetivity)
    {
        float rVal = 0;
        if (Input.GetKey(KeyCode.S))
        {
            rVal -= 1f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            rVal += 1f;
        }
        return rVal * sensetivity;
    }

    protected bool GetActiveationInput()
    {
        return Input.GetKey(KeyCode.Space);
    }
    #endregion

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(!controller.Grounded && activeInput() && fuel > 0)
        {
            rb.AddForce(transform.up*upForce*Time.fixedDeltaTime);
            fuel -= burnRate * Time.fixedDeltaTime;
            rb.drag += activatedDragDelta - lastDragDelta;
            lastDragDelta = activatedDragDelta;
        }
        else
        {
            rb.drag -= lastDragDelta;
            lastDragDelta = 0f;
        }
        if (!controller.Grounded && fuel > 0)
        {
            float zInputVal = zInput(1);
            float xInputVal = xInput(1);
            fuel -= zBurnRate * Mathf.Abs(zInputVal) * Time.fixedDeltaTime;
            Vector3 forwards = controller.cam.transform.forward + controller.cam.transform.up *.01f;
            forwards.y = 0;
            forwards = forwards.normalized;
            rb.AddForce(forwards * zForce * zInputVal * Time.fixedDeltaTime);
            fuel -= xBurnRate * Mathf.Abs(xInputVal) * Time.fixedDeltaTime;
            rb.AddForce(controller.cam.transform.right * xForce * xInputVal * Time.fixedDeltaTime);
        }

        fuel += regenerationRate * Time.fixedDeltaTime;
        fuel = Mathf.Min(fuel, maxFuel);
	}
}
