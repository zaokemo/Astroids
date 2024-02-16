using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyedParticles;
    public int size = 3;

    public GameManager gameManager;
 

    private void Start()
    {
        // Scale based on the size.
        transform.localScale = 0.5f * size * Vector3.one;

        // Add movement, bigger asteroids are slower.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = Random.insideUnitCircle.normalized;
        float spawnSpeed = Random.Range(4f - size, 5f - size);
        rb.AddForce(direction * spawnSpeed, ForceMode2D.Impulse);

        // Register creation
        if (gameManager != null)
        {
            gameManager.asteroidCount++;
            Debug.Log("Asteroid Count: " + gameManager.asteroidCount);
        }
        else
        {
            Debug.LogWarning("GameManager not assigned to the Asteroid.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asteroids are only destroyed with bullets.
        if (collision.CompareTag("Bullet")) // Corrected typo in CompareTag
        {
            // Register the destruction with the game manager.
            gameManager.asteroidCount--;

            // Destroy the bullet so it doesn't carry on and hit more things.
            Destroy(collision.gameObject);

            // If size > 1 spawn 2 smaller asteroids of size-1.
            if (size > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity); // Corrected typo in Transform
                    newAsteroid.size = size - 1;
                    newAsteroid.gameManager = gameManager;
                }
            }

            // Spawn particles on destruction.
            if (destroyedParticles != null) 
            {
                Instantiate(destroyedParticles, transform.position, Quaternion.identity);
            }
            
            // Destroy this asteroid.
            Destroy(gameObject);
            gameManager.IncreaseScore();
        }
    }
}
