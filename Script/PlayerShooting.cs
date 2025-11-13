using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 30f;  // Increased speed
    public float fireRate = 0.2f;
    
    private float nextFireTime = 0f;

    void Update()
    {
        // Debug line
        Debug.DrawRay(firePoint.position, firePoint.forward * 50f, Color.red);
        
        // Shoot
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Apply velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
            rb.useGravity = false; // No gravity = flies straight!
        }
        
        Destroy(bullet, 5f);
        
        Debug.Log("BANG!");
    }
}