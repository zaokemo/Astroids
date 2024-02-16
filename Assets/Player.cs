using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Ship parameters")]
    [SerializeField] private float shipAcceleration = 10f;
    [SerializeField] private float shipMaxVelocity = 10f;
    [SerializeField] private float shipRotationSpeed = 108f;
    [SerializeField] private float bulletSpeed = 8f;

    [Header("Object reference")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private ParticleSystem destroyedParticles; // Add this line

    private Rigidbody2D shipRigidbody;
    private bool isAlive = true;
    private bool isAccelerating = false;

    private void Start()
    {
        // Get a reference to the attached Rigidbody2D.
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAlive)
        {
            HandleShipMovement();
            HandleShipRotation();
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        // Optional: You can add movement code here if needed for physics calculations.
    }

    private void HandleShipMovement()
    {
        // Move ship forward with W key
        if (Input.GetKey(KeyCode.W))
        {
            // Apply force in the forward direction of the ship
            shipRigidbody.AddForce(shipAcceleration * transform.up);
        }

        // Rotate ship with A and D keys
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * shipRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * -shipRotationSpeed * Time.deltaTime);
        }

        // Limit the ship's velocity
        shipRigidbody.velocity = Vector2.ClampMagnitude(shipRigidbody.velocity, shipMaxVelocity);
    }

    private void HandleShipRotation()
    {
        // Rotate ship towards the mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 directionToMouse = mousePosition - transform.position;
        directionToMouse.z = 0; // Ensure the ship stays in the 2D plane

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandleShooting()
    {
        // Shooting on left-click (GetMouseButtonDown(0))
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            Debug.Log("Left mouse button clicked!");

            // Instantiate a bullet at the bulletSpawn position
            Rigidbody2D bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

            // Set the initial velocity of the bullet based on ship's rotation
            bullet.velocity = bulletSpeed * transform.up;

            // Add force to propel the bullet in the direction the player is facing
            bullet.AddForce(bulletSpeed * transform.up, ForceMode2D.Impulse);
        }
        // You can keep the right-click logic for any specific behavior you want.
        else if (Input.GetMouseButtonDown(1)) // 1 corresponds to the right mouse button
        {
            Debug.Log("Right mouse button clicked!");
            // Rest of the shooting code for right-click
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Asteroid")) {
            isAlive = false;

            // Get a reference to the GameManager
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            // restart game after delay.
            gameManager.GameOver();

            // Show the destroyed effect.
            Instantiate(destroyedParticles, transform.position, Quaternion.identity);

            // Destroy the player.
            Destroy(gameObject);
        }

        if (collision.CompareTag("Asteroid"))
        {
            isAlive = false;

            // Get a reference to the GameManager
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            // Decrease lives when the player is hit
            gameManager.DecreaseLives();

            // ... (rest of the code)
        }
    }
}

