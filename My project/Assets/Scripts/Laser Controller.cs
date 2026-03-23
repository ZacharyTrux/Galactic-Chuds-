using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float warningDuration = 1.2f;
    public float activeDuration = 2f;
    
    private SpriteRenderer sprite;
    private Collider2D collider;

    void Awake(){
        sprite = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        collider.enabled = false; // turn off collision
    }   

    void Start(){
        StartCoroutine(LaserRoutine());
    }

    System.Collections.IEnumerator LaserRoutine(){
        float elapsedTime = 0;
        while(elapsedTime <= warningDuration){
            float alphaVal = Mathf.PingPong(Time.time * 5f, 0.9f) + 0.1f;
            sprite.color = new Color(1, 1, 1, alphaVal);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = new Color(1,1,1,1);
        collider.enabled = true;
        yield return new WaitForSeconds(activeDuration);

        Destroy(gameObject);
    }
}
