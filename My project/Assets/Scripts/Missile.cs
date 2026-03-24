using UnityEngine;

public class Missile : MonoBehaviour {
    public float speed = 10f;
    public float turnSpeed = 400f;

    private Transform target;

    void Start() {
        FindNearestTarget();
    }

    void Update() {
        if (target == null) {
            FindNearestTarget(); // keep searching if target was destroyed
        }

        if (target != null) {
            // get the direction from missile to target as a normalized vector
            Vector2 direction = (target.position - transform.position).normalized;
            // convert direction to an angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // smoothly rotate toward that angle at turnSpeed degrees per second
            float newAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

        // always move forward in the direction the missile is facing
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // destroy missile if it goes off screen
        if (transform.position.x > 12f || transform.position.x < -12f) {
            Destroy(gameObject);
        }
    }

    void FindNearestTarget() {
        // get all objects tagged Enemy in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDist = Mathf.Infinity;
        target = null;

        foreach (GameObject enemy in enemies) {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist) { // track whichever enemy is closest
                closestDist = dist;
                target = enemy.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss")) {
            Destroy(gameObject);
        }
    }
}
