using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime); // move right over in game time

        if (transform.position.x > 9f) { // destroy when off screen
            Destroy(gameObject);
        }
    }
}
