using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour 
{
    public GameObject destroyedVersion;  // Reference to the shattered version of the object
    public float explosionForce = 5f;    // Force applied to shattered pieces
    public float explosionRadius = 2f;   // Radius of explosion effect
    public float breakVelocity = 3f;     // Minimum velocity to break on impact

    // When something collides with the bottle
    void OnCollisionEnter(Collision collision)
    {
        bool shouldBreak = false;
        Vector3 impactPoint = collision.contacts[0].point;

        // Check if the collision is with a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            shouldBreak = true;
            Debug.Log("Bottle destroyed by bullet!");
        }
        // Check if bottle fell and hit something hard enough
        else if (collision.relativeVelocity.magnitude >= breakVelocity)
        {
            shouldBreak = true;
            Debug.Log("Bottle shattered from fall impact! Velocity: " + collision.relativeVelocity.magnitude);
        }

        // Break the bottle if conditions are met
        if (shouldBreak)
        {
            BreakBottle(impactPoint);
        }
    }

    void BreakBottle(Vector3 impactPoint)
    {
        // Spawn the shattered version at this position
        GameObject shattered = Instantiate(destroyedVersion, transform.position, transform.rotation);
        
        // Apply explosion force to all shattered pieces (if they have Rigidbodies)
        Rigidbody[] pieces = shattered.GetComponentsInChildren<Rigidbody>();
        
        foreach (Rigidbody piece in pieces)
        {
            // Add force away from impact point
            piece.AddExplosionForce(explosionForce, impactPoint, explosionRadius);
        }
        
        // Destroy shattered object after 3 seconds (cleanup)
        Destroy(shattered, 3f);
        
        // Remove the intact bottle
        Destroy(gameObject);
    }
}