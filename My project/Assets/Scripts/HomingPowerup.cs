using UnityEngine;

public class HomingPowerup : MonoBehaviour {
    public float speed = 3f;

    void Update() {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x < -12f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (GameManager.Instance.currState == GameState.Boss) {
                other.GetComponentInParent<Player>().ActivateHomingShots(1);
            } else {
                other.GetComponentInParent<Player>().ActivateHomingShots(3);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
