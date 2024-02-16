using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Asteroid asteroidPrefab;
    public int asteroidCount = 0;
    private int level = 0;
    public Text scoreText;
    public int score;
    public Text livesText;  // Add this line
    public static int lives = 3;  // Add this line
    
    
    private void Update()
    {
        // If there are no asteroids left, spawn more!
        if (asteroidCount == 0)
        {
            // Increase the level.
            level++;

            // Spawn the correct number for this level.
            // 1=>4, 2=>6, 3=>6, 4=>10 ...
            int numAsteroids = 2 + (2 * level);
            Debug.Log("Spawning " + numAsteroids + " asteroids for level " + level);
            for (int i = 0; i < numAsteroids; i++)
            {
                SpawnAsteroid();
            }
        }
    }

private void SpawnAsteroid()
{
    // How far along the edge.
    float minOffset = 0.2f; // Minimum offset from the center (20%)
    float maxOffset = 0.8f; // Maximum offset from the center (80%)
    float offset = Random.Range(minOffset, maxOffset);

    Vector2 viewportSpawnPosition = Vector2.zero;

    // Which edge.
    int edge = Random.Range(0, 4);
    if (edge == 0)
    {
        viewportSpawnPosition = new Vector2(offset, 0);
    }
    else if (edge == 1)
    {
        viewportSpawnPosition = new Vector2(offset, 1);
    }
    else if (edge == 2)
    {
        viewportSpawnPosition = new Vector2(0, offset);
    }
    else if (edge == 3)
    {
        viewportSpawnPosition = new Vector2(1, offset);
    }

    // Create the asteroid.
    Vector2 worldSpawnPosition = Camera.main.ViewportToWorldPoint(viewportSpawnPosition);
    Debug.Log("Spawning asteroid at: " + worldSpawnPosition);
    Asteroid asteroid = Instantiate(asteroidPrefab, worldSpawnPosition, Quaternion.identity);
    asteroid.gameManager = this;  // Assign the game manager reference to the spawned asteroid
}


    public void GameOver()
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        Debug.Log("Game Over");

        // Wait a bit before restarting.
        yield return new WaitForSeconds(2f);
        
    if (lives > 0)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    else
    {
        SceneManager.LoadScene("GameOverScene"); // Replace with your actual game-over scene name
        Destroy(gameObject);
    }

        yield return null;
    }

    public void IncreaseScore(){
        score++;
        scoreText.text = score.ToString();
    }

     void Start()
    {
        UpdateLives();  // Add this line to update lives text on start
    }

    private void UpdateLives()  // Add this method
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    public void DecreaseLives()  // Add this method
    {
        lives--;
        UpdateLives();
        if (lives <= 0)
        {
            lives = 0; // Ensure lives don't go below zero
            GameOver();
        }
    }

    
}
