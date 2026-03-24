using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 9f) {
            Destroy(gameObject);
        }
    }
}
