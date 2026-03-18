using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class EnemyController : MonoBehaviour {
    // set in inspector
    public float speed = 4f;
    public float destroyAtX = -11f;
    private const float awardedPoints = 500.0f; 

    private void Update() {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < destroyAtX) {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Score.Instance.UpdateScore(awardedPoints);
        }
        else if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
            Destroy(gameObject);
        }
    }
}
