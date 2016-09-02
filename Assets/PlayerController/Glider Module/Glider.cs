using UnityEngine;
using System.Collections;

public class Glider : MonoBehaviour {

    public Transform cameraTransform;//used to determin what direction to go
    public AnimationCurve degreesVsRoll;//how much the glider tries to roll at various degrees from target
    public AnimationCurve speedVsmaxVelocityDelta;
    public float rollRate = 100f;
    public float pitchRate = 60f;
    public float yawRate = 40f;
    public float cameraTargetRate = 1f;//how agressively the glider tries to match the direction of the camera
    public Rigidbody rb;

    //these controls override camera transform when not = 0
    protected FloatyInput rollInput;
    protected FloatyInput pitchInput;
    protected FloatyInput yawInput;

    // Use this for initialization
    void Start () {
        SetupInputDelegates();
        rb = GetComponent<Rigidbody>();
	}

    #region setupInputs
    /// <summary>
    /// Sets up deliages if they aren't set yet, makes it easier to change to make changes without changing this script (example: a temp control lock can be done easily by a seperate component that changes the delegatesfor the needed time) 
    /// </summary>
    void SetupInputDelegates()
    {
        if (rollInput == null)
        {
            rollInput = GetRollInput;
        }
        if (pitchInput == null)
        {
            pitchInput = GetPitchInput;
        }
        if (yawInput == null)
        {
            yawInput = GetYawInput;
        }
    }

    protected float GetRollInput(float sensetivity)
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

    protected float GetPitchInput(float sensetivity)
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

    protected float GetYawInput(float sensetivity)
    {
        float rVal = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            rVal -= 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rVal += 1f;
        }
        return rVal * sensetivity;
    }
    #endregion
	
    void FixedUpdate()
    {
        ProcessTurning();
        ProcessVelocityTurning();
    }

    void ProcessVelocityTurning()
    {
        float speed = rb.velocity.magnitude;
        Vector3 newVelocity = transform.forward * speed;
        float maxDis = speedVsmaxVelocityDelta.Evaluate(speed) * Time.fixedDeltaTime;
        //Debug.Log("Slip Speed: " + (speed-Vector3.Project(rb.velocity, transform.forward).magnitude) + " Speed: " + speed + " Speed Along Forwards" + Vector3.Project(rb.velocity,transform.forward).magnitude + " Speed Delta: " + speedVsmaxVelocityDelta.Evaluate(speed) + "Velocity: " + rb.velocity);
        if (maxDis >= Vector3.Distance(newVelocity, rb.velocity))
        {
            //newVelocity = rb.velocity;
        } else
        {
            newVelocity = Vector3.Slerp(rb.velocity,newVelocity,maxDis/ Vector3.Distance(newVelocity, rb.velocity));
        }
        rb.velocity = newVelocity;
    }

	void ProcessTurning ()
    {
        Vector3 myForwards = transform.forward;
        myForwards.y = 0;
        Vector3 camForwards = cameraTransform.forward;
        camForwards.y = 0;
        Vector3 rotation = transform.rotation.eulerAngles;
        float angle = Vector3.Angle(myForwards, camForwards);
        float roll = -rollInput(rollRate * Time.fixedDeltaTime);
        float pitch = pitchInput(pitchRate * Time.fixedDeltaTime);
        float yaw = yawInput(yawRate * Time.fixedDeltaTime);
        Vector3 reletiveTarget = transform.InverseTransformDirection(cameraTransform.forward).normalized;
        if (roll == 0f)
        {
            Quaternion oldRotation = transform.rotation;
            transform.rotation = Quaternion.LookRotation(transform.forward);
            reletiveTarget = transform.InverseTransformDirection(cameraTransform.forward).normalized;
            float targetRoll = degreesVsRoll.Evaluate(angle);
            if (reletiveTarget.x >= 0)
            {
                targetRoll *= -1;
            }
            
            float rotZ = rotation.z;
            if (rotZ > 180)
            {
                rotZ = rotZ - 360;
            }else if(rotZ < -180)
            {
                rotZ = rotZ + 360;
            }
            roll = (targetRoll - rotZ);
            //Debug.Log("Target Roll: " + targetRoll + " Roll Rotation:" + rotZ + " Roll: " + roll);
            roll = Mathf.Clamp(Mathf.Clamp(roll,-1,1) * Time.fixedDeltaTime * rollRate,-Mathf.Abs(roll), Mathf.Abs(roll));
            //Debug.Log("Final Roll: " + roll);
            transform.rotation = oldRotation;
        }
        transform.Rotate(0, 0, roll, Space.Self);
        reletiveTarget = transform.InverseTransformDirection(cameraTransform.forward).normalized;
        if (pitch == 0f)
        {
            pitch = Mathf.Clamp(reletiveTarget.y * cameraTargetRate, -1f, 1f) * -pitchRate * Time.fixedDeltaTime;
            //TODO if angle is close enough set pitch to directly face target direction
        }
        if (yaw == 0f)
        {
            yaw = Mathf.Clamp(reletiveTarget.x * cameraTargetRate, -1f, 1f) * yawRate * Time.fixedDeltaTime;
            //TODO if angle is close enough set pitch to directly face target direction
        }
        transform.Rotate(pitch, yaw, 0, Space.Self);
    }
}
