using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f; // Bullets live 2 seconds after hitting
    private bool hasHit = false;

    void Start()
    {
        // Destroy after 5 seconds if it never hits anything
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            hasHit = true;
            
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            
            // Destroy after short delay (lets it bounce first!)
            Destroy(gameObject, lifetime);
        }
    }
}