using UnityEngine;

public class Spaceship : MonoBehaviour {
    // set in inspector
    public float VerticalSpeed = 3f;
    public float HorizontalSpeed = 0.5f;
    public GameObject enemyBulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 2f;
    private float timeUntilNextShot = 0f;
    private const float Y_LIMIT = 4.6f;
    private bool isMovingUp = true;
    public float destroyAtX = -11f;

    private void Update() {
        
        // Spaceship Movement

        transform.Translate(Vector2.left * HorizontalSpeed * Time.deltaTime);

        if (isMovingUp == true) {
            transform.Translate(Vector2.up * VerticalSpeed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.down * VerticalSpeed * Time.deltaTime);
        }

        if (transform.position.y > Y_LIMIT) {
            isMovingUp = false;
        } else if (transform.position.y < -Y_LIMIT) {
            isMovingUp = true;
        }
         if (transform.position.x < destroyAtX) {
            Destroy(gameObject);
        }

        
// 1. Tick the timer down every frame
        timeUntilNextShot -= Time.deltaTime;

        // 2. If the timer hits 0 (or goes below), shoot!
        if (timeUntilNextShot <= 0f) {
            Instantiate(enemyBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            
            // 3. Reset the timer back to your fireRate (2 seconds)
            timeUntilNextShot = fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if (other.CompareTag("Player")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
