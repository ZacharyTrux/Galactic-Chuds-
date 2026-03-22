using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    // set in inspector
    public float speed = 6f;

    private void Update() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < -12f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
            Destroy(gameObject);
        }
    }
}
