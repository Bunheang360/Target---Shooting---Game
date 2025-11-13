using UnityEngine;

public class SwingingTarget : MonoBehaviour
{
    public float impactMultiplier = 15f;
    public float torqueMultiplier = 5f; // NEW: Makes it spin/wobble
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Get impact point and direction
            Vector3 impactDirection = collision.contacts[0].normal * -1;
            Vector3 impactPoint = collision.contacts[0].point;
            
            // Apply force at impact point (more realistic!)
            rb.AddForceAtPosition(impactDirection * impactMultiplier, impactPoint, ForceMode.Impulse);
            
            // Add some torque to make it spin/wobble
            Vector3 torque = new Vector3(
                Random.Range(-torqueMultiplier, torqueMultiplier),
                0f,
                Random.Range(-torqueMultiplier, torqueMultiplier)
            );
            rb.AddTorque(torque, ForceMode.Impulse);
            
            Debug.Log("Target Hit! POW!");
        }
    }
}