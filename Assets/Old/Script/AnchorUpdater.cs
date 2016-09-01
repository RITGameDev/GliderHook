using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// DO NOT USE, IS ONLY A REFRENCE FROM DIFFRENT PROJECT
/// </summary>
public class AnchorUpdater : MonoBehaviour
{

    public Vector3 worldAnchorPosition;
    //public Vector3 worldAxis;
    public Vector3 worldSwingAxis;
    public ConfigurableJoint joint;

    public Rigidbody rigidbody;
    public float distance;
    public bool bAnchored;
    public GameObject grapleCableVisualPrefab;
    private GameObject grapleCable;
    public LayerMask cablemask;
    public float spaceContractRate = 0f;
    private Vector3 lastVelocity;
    private Queue<GameObject> cables = new Queue<GameObject>();
    void Update()
    {
        if (bAnchored)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                distance -= spaceContractRate * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                distance += spaceContractRate * Time.deltaTime;
            }
            FindNewInterSection();
            UpdateCabel();
        }
    }

    void FixedUpdate()
    {
        if (bAnchored)
        {
            DummyUpdate();
        }
        /*if ((lastVelocity - rigidbody.velocity).magnitude > 8.0f)
        {
            Debug.Log(rigidbody.velocity);
            Debug.Break();
        }
        lastVelocity = rigidbody.velocity;
        Debug.Log(rigidbody.velocity);
        Debug.Log("Actual Distance: " + (rigidbody.transform.position - worldAnchorPosition).magnitude + " Distance: " + distance);//*/
    }

    void FindNewInterSection()
    {
        RaycastHit hit;
        if (Physics.Raycast(rigidbody.transform.position, worldAnchorPosition - rigidbody.transform.position, out hit, (worldAnchorPosition - rigidbody.transform.position).magnitude - 0.2f, cablemask))
        {
            Debug.Log("New AnchorPoint");
            GameObject newCable = Instantiate(grapleCableVisualPrefab);
            newCable.transform.position = (worldAnchorPosition + hit.point) / 2f;
            newCable.transform.LookAt(hit.point);
            newCable.transform.localScale = new Vector3(0.1f, 0.1f, (worldAnchorPosition - hit.point).magnitude);
            cables.Enqueue(newCable);
            if (cables.Count > 15)
            {
                Destroy(cables.Dequeue());
            }
            worldAnchorPosition = hit.point;
            joint.connectedAnchor = hit.point;
            distance = (hit.point - rigidbody.transform.position).magnitude;
            SoftJointLimit lim = joint.linearLimit;
            lim.limit = distance;
            joint.linearLimit = lim;
            UpdateCabel();
        }
    }

    void DummyUpdate()
    {
        if ((rigidbody.transform.position - worldAnchorPosition).magnitude < distance && !Input.GetKey(KeyCode.LeftShift))
        {
            distance = (rigidbody.transform.position - worldAnchorPosition).magnitude;// + 0.01f;
        }
        else
        {
            //distance += 0.001f;
        }
        distance = Mathf.Clamp(distance, 2f, 100f);
        SoftJointLimit lim = joint.linearLimit;
        lim.limit = distance;
        joint.linearLimit = lim;
    }

    private void UpdateCabel()
    {
        grapleCable.transform.position = (rigidbody.transform.position + rigidbody.transform.right + worldAnchorPosition) / 2f;
        grapleCable.transform.LookAt(worldAnchorPosition);
        grapleCable.transform.localScale = new Vector3(0.1f, 0.1f, (rigidbody.transform.position - worldAnchorPosition).magnitude);
        //grapleCable.transform.Translate(rigidbody.transform.right * 0.5f, Space.World);
    }

    public void NewAnchor(RaycastHit hit)
    {
        if (!bAnchored)
        {
            //rigidbody.constraints = RigidbodyConstraints.None;
            joint = rigidbody.gameObject.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = hit.point;
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            SoftJointLimit lim = joint.linearLimit;
            distance = (hit.point - rigidbody.transform.position).magnitude;
            lim.limit = distance;
            joint.linearLimit = lim;
            joint.projectionMode = JointProjectionMode.PositionAndRotation;
            worldAnchorPosition = hit.point;
            bAnchored = true;
            grapleCable = Instantiate(grapleCableVisualPrefab) as GameObject;
            UpdateCabel();
        }
    }

    public void RemoveAnchor()
    {
        if (bAnchored)
        {
            Destroy(grapleCable);
            Destroy(joint);
            bAnchored = false;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            /*while(cables.Count > 0)
            {
                Destroy(cables.Dequeue());
            }//*/
        }
    }
}
