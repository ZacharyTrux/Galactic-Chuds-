using UnityEngine;

public class EnemyController : MonoBehaviour {
    // set in inspector
    public float speed = 4f;
    public float destroyAtX = -11f;

    private void Update() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < destroyAtX) {
            Destroy(gameObject);
        }
    }
        private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
            stats.UpdateScore(50);
        }
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerStats>().DamagePlayer();
            Destroy(gameObject);
        }
    }
}
