using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour
{
    public float maxDistance = 1000;
    public float maxRetractionForce;
    public float sprint;
    public float dampaning;
    ConfigurableJoint joint;
    public float retractionRate;
    [HideInInspector]public Rigidbody rigidbody;
    public LayerMask grappleMask;
    public Transform look;
    public Transform grapleCable;
    [HideInInspector] public Vector3 worldAnchorPosition;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (joint)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, joint.connectedAnchor - transform.position, out hit, 300, grappleMask))
            {
                if ((hit.point - joint.connectedAnchor).magnitude > 0.5f)
                {
                    Destroy(joint);
                }
            }
            grapleCable.transform.position = (rigidbody.transform.position + rigidbody.transform.right + worldAnchorPosition) / 2f;
            grapleCable.transform.LookAt(worldAnchorPosition);
            grapleCable.transform.localScale = new Vector3(0.1f, 0.1f, (rigidbody.transform.position - worldAnchorPosition).magnitude);
        }
        else
        {
            grapleCable.transform.position = Vector3.down * 1000;
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(look.position, look.forward, out hit, 290, grappleMask))
                {
                    NewAttachmentPoint(hit);
                }
            }
        }
    }

    void FixedUpdate()
    {
        SoftJointLimit limit;
        if (joint && (transform.position - joint.connectedAnchor).magnitude < joint.linearLimit.limit)
        {
            limit = joint.linearLimit;
            limit.limit = (joint.connectedAnchor - transform.position).magnitude;
            //joint.linearLimit = limit;
        }
        if (joint)
        {
            limit = joint.linearLimit;
            limit.limit = Mathf.Clamp((joint.connectedAnchor - transform.position).magnitude - (retractionRate * Time.deltaTime),0, 300);
            //joint.linearLimit = limit;

            if (limit.limit < 5)
            {
                Destroy(joint);
            }
        }
        if (joint && Input.GetMouseButtonDown(1))
        {
            Destroy(joint);
        }
        if (joint)
        {
            Debug.Log(joint.linearLimit.limit);
        }
    }

    public void NewAttachmentPoint(RaycastHit hit)
    {
        if (joint)
        {
            Destroy(joint);
        }
        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = hit.point;// transform.InverseTransformPoint(hit.point);
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        worldAnchorPosition = hit.point;
        SoftJointLimit limit = joint.linearLimit;
        limit.limit = 300;// (hit.point - transform.position).magnitude;
        joint.linearLimit = limit;
        JointDrive jdrive = joint.xDrive;
        jdrive.maximumForce = 400;
        jdrive.positionDamper = 3f;
        jdrive.positionSpring = 100;
        joint.xDrive = jdrive;
        joint.yDrive = jdrive;
        joint.zDrive = jdrive;
        SoftJointLimitSpring spring = joint.linearLimitSpring;
        spring.spring = 100;
        joint.linearLimitSpring = spring;
        joint.breakForce = 1000;
        joint.breakTorque = 1000;
    }
}
