using UnityEngine;

public class Powerup : MonoBehaviour {
  // set in inspector
  public float speed;

  void Update() {
    transform.Translate(Vector3.left * speed * Time.deltaTime);

    if (transform.position.x < -12f) {
            Destroy(gameObject);
        }
  }

  private void OnTriggerEnter2D(Collider2D c) {
    if (c.gameObject.CompareTag("Player Bullet")) {
      Destroy(gameObject);
      Destroy(c.gameObject);
    }
    else if (c.gameObject.CompareTag("Player")) {
      Destroy(gameObject);
      c.gameObject.GetComponent<Player>().RefillShield();
    }
  }
}
