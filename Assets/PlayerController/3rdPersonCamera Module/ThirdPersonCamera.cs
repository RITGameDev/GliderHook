using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{

    public Transform target;
    public LayerMask collisionMask = int.MaxValue;
    public float sphereTraceRadise = .2f;
    public float MinDistance = 1f;
    public float MaxDistance = 10f;
    public float distanceStep = 1f;
    public float xSensativity = 1f;
    public float ySensativity = 1f;

    protected FloatyInput xInput;
    protected FloatyInput yInput;
    protected FloatyInput zoomInput;

    private float lastDistance;
    private Vector3 lastRotation;

    void Start()
    {
        Debug.Log("Start");
        SetupInputDelegates();
        lastRotation = transform.rotation.eulerAngles;
        lastRotation.z = 0;
        lastDistance = Vector3.Distance(target.position, transform.position);
    }

    #region setupInputs
    /// <summary>
    /// Sets up deliages if they aren't set yet, makes it easier to change to make changes without changing this script (example: a temp control lock can be done easily by a seperate component that changes the delegatesfor the needed time) 
    /// </summary>
    void SetupInputDelegates()
    {
        if(xInput == null)
        {
            xInput = GetXInput;
        }
        if (yInput == null)
        {
            yInput = GetYInput;
        }
        if (zoomInput == null)
        {
            zoomInput = GetZoomInput;
        }
    }

    protected float GetXInput(float sensetivity)
    {
        return Input.GetAxis("Mouse X") * sensetivity;
    } 

    protected float GetYInput(float sensetivity)
    {
        return Input.GetAxis("Mouse Y") * sensetivity;
    }

    protected float GetZoomInput(float sensetivity)
    {
        return Input.GetAxis("Mouse ScrollWheel") * 10f * sensetivity;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        lastRotation.z = 0;

        //stupid x => y conversion here because of how unity has its axis setup for 2D vs 3D
        lastRotation.x -= yInput(ySensativity);
        lastRotation.x = Mathf.Clamp(lastRotation.x, -90, 90);
        lastRotation.y += xInput(xSensativity);
        transform.rotation = Quaternion.Euler(lastRotation);

        lastDistance -= zoomInput(distanceStep);
        lastDistance = Mathf.Clamp(lastDistance, MinDistance, MaxDistance);
        float distance = lastDistance;
        RaycastHit hit;
        if(Physics.SphereCast(transform.position,sphereTraceRadise,-transform.forward,out hit, lastDistance, collisionMask))
        {
            if(Vector3.Distance(hit.point,transform.position) < lastDistance)
            {
                distance = Vector3.Distance(hit.point, transform.position);
            }
        }
        transform.Translate(0, 0,-distance, Space.Self);
    }


}
