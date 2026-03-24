using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 12f) {
            Destroy(gameObject);
        }
    }
}
