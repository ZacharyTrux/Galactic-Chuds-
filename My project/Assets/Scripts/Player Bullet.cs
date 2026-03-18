using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > 12f) {
            Destroy(gameObject);
        }
    }
}
