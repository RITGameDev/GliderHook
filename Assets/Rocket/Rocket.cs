using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{

    public float randomRotationVelRange = 10f;
    public float startingVelocity = 50f;
    public float acceleration = 10f;
    public float velocityTurnMaxAngle = 1f;
    public float killImpactVelocity = 30f;
    public float killTime = 1f;

    protected Rigidbody rb;
    protected float time = 0f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * startingVelocity;
        rb.angularVelocity = new Vector3(Random.Range(-randomRotationVelRange, randomRotationVelRange), Random.Range(-randomRotationVelRange, randomRotationVelRange), Random.Range(-randomRotationVelRange, randomRotationVelRange));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * Time.fixedDeltaTime * acceleration, ForceMode.Acceleration);
        rb.velocity = Statics.VelocityTurn(rb.velocity, transform.forward, velocityTurnMaxAngle * Time.fixedDeltaTime);
        time += Time.fixedDeltaTime;
        if(time > killTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Got Collision Event, reletive Velocty: " + collision.relativeVelocity + " Normal:" + collision.contacts[0].normal);
        Vector3 velInNormal = Vector3.Project(collision.relativeVelocity, collision.contacts[0].normal);
        Debug.Log("Velocity along normal: " + velInNormal.magnitude);
        if(velInNormal.magnitude > killImpactVelocity)
        {
            Destroy(gameObject);
        }
    }
}
