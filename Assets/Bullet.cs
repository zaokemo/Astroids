using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 1f;

    // When created, destroy after a set period of time.
    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
    }

    // Check for collisions with asteroids
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);  // Destroy the asteroid
            Destroy(gameObject);  // Destroy the bullet
        }
    }

    // Called when the renderer is no longer visible from any camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
