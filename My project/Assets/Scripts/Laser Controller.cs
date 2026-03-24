using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float warningDuration = 1.2f;
    public float activeDuration = 2f;
    public AudioClip laserCharge;
    public AudioClip laserFire;
    
    private SpriteRenderer sprite;
    private Collider2D laserCollider;
    private AudioSource audioSrc;

    void Awake(){
        sprite = GetComponentInChildren<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();

        laserCollider.enabled = false; // turn off collision
    }   

    void Start(){
        audioSrc = GetComponent<AudioSource>();
        StartCoroutine(LaserRoutine());
    }

    System.Collections.IEnumerator LaserRoutine(){
        float elapsedTime = 0;
        audioSrc.clip = laserCharge;
        audioSrc.Play();
        while(elapsedTime <= warningDuration){
            float alphaVal = Mathf.PingPong(Time.time * 5f, 0.9f) + 0.1f;
            sprite.color = new Color(1, 1, 1, alphaVal);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSrc.clip = laserFire;
        audioSrc.pitch = laserFire.length / activeDuration;
        audioSrc.Play();
        sprite.color = new Color(1,1,1,1);
        laserCollider.enabled = true;
        yield return new WaitForSeconds(activeDuration);

        float fadeTime = 0.2f;
        float startVolume = audioSrc.volume;
        while (audioSrc.volume > 0)
        {
            audioSrc.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (!player.shield.IsActive) { // only damage player if shield is not active
                player.DamageFromEnemy();
            }
        }
    }
}
