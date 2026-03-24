using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float speed = 6f;

    private void Update(){
        transform.Translate(Vector2.left * speed * Time.deltaTime); // move left over in game time

        if (transform.position.x < -11f) { // delete when out of bounds
            Destroy(gameObject);
        }
    }

<<<<<<< Updated upstream
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Shield")) {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Player")) {
=======
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")) { // detect player collision
>>>>>>> Stashed changes
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
            Destroy(gameObject);
        }
    }
}
