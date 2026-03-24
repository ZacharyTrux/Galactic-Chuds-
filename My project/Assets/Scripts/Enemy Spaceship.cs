using UnityEngine;

public class Spaceship : MonoBehaviour {
    // set in inspector
    public float VerticalSpeed = 3f;
    public float HorizontalSpeed = 0.5f;
    public GameObject enemyBulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 2f;
    public float destroyAtX = -11f;
    public GameObject expoPrefab;
    public AudioClip shootingAudio;
    public AudioClip explosionAudio;

    // private variables
    private const float awardedPoints = 1000.0f;
    private bool isMovingUp = true;
    private float timeUntilNextShot = 0f;
    private const float Y_LIMIT = 4.6f;
    private AudioSource audioSrc;

    private void Start(){
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update() {
        // Spaceship Movement
        transform.Translate(Vector2.left * HorizontalSpeed * Time.deltaTime);

        if (isMovingUp == true) {
            transform.Translate(Vector2.up * VerticalSpeed * Time.deltaTime);
        } 
        else {
            transform.Translate(Vector2.down * VerticalSpeed * Time.deltaTime);
        }

        if (transform.position.y > Y_LIMIT) {
            isMovingUp = false;
        } 
        else if (transform.position.y < -Y_LIMIT) {
            isMovingUp = true;
        }
        if (transform.position.x < destroyAtX) {
            Destroy(gameObject);
        }

        
// 1. Tick the timer down every frame
        timeUntilNextShot -= Time.deltaTime;

        // 2. If the timer hits 0 (or goes below), shoot!
        if (timeUntilNextShot <= 0f && transform.position.x >= 9) {
            Instantiate(enemyBulletPrefab, bulletSpawnPoint.position, UnityEngine.Quaternion.identity);
            audioSrc.clip = shootingAudio;
            audioSrc.Play();
            
            // 3. Reset the timer back to your fireRate (2 seconds)
            timeUntilNextShot = fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player Bullet")){
            Destroy(other.gameObject);
            Score.Instance.UpdateScore(awardedPoints);
            AudioSource.PlayClipAtPoint(explosionAudio, transform.position);

            var expoObj = Instantiate(expoPrefab, transform.position, UnityEngine.Quaternion.identity); // creates explosion of enemy object
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
            Destroy(gameObject);
        }

        else if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
            Destroy(gameObject);
        }
    }
}
