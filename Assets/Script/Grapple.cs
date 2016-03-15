using UnityEngine;
using System.Collections;

public class Grapple : MonoBehaviour
{
    public float maxDistance = 1000;
    public float maxRetractionForce;
    public float minDistance = 100;
    public float force = 100;
    public float forceYscale;
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
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (joint)
            {
                Destroy(joint);
            }
            RaycastHit hit;
            if (Physics.Raycast(look.position, look.forward, out hit, maxDistance, grappleMask))
            {
                NewAttachmentPoint(hit);
            }
        }
    }

    void FixedUpdate()
    {
        if (joint)
        {
            rigidbody.AddForce(grapleCable.forward * force);
        }
        if (joint && (Input.GetMouseButtonDown(1) || (transform.position - joint.connectedAnchor).magnitude < minDistance))
        {
            Destroy(joint);
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
        joint.connectedAnchor = hit.point;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;
        worldAnchorPosition = hit.point;
        SoftJointLimit limit = joint.linearLimit;
        limit.limit = maxDistance;
        joint.linearLimit = limit;
        SoftJointLimitSpring spring = joint.linearLimitSpring;
        spring.spring = 100;
        joint.linearLimitSpring = spring;
        joint.breakForce = 1000;
        joint.breakTorque = 1000;
    }
}
