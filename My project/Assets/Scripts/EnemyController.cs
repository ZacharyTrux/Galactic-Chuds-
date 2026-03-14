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
}
