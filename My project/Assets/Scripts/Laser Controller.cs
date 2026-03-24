using UnityEngine;

public class LaserController : MonoBehaviour{
    // public, inspector variables
    public float warningDuration = 1.2f;
    public float activeDuration = 2f;
    public AudioClip laserChargeAudio;
    public AudioClip laserFireAudio;
    
    // private variables
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
        StartCoroutine(LaserRoutine()); // start a sub-routine
    }

    System.Collections.IEnumerator LaserRoutine(){
        float elapsedTime = 0;
        audioSrc.clip = laserChargeAudio;
        audioSrc.Play();
        while(elapsedTime <= warningDuration){
            float alphaVal = Mathf.PingPong(Time.time * 5f, 0.9f) + 0.1f; // go back and forth between alpha values
            sprite.color = new Color(1, 1, 1, alphaVal);
            elapsedTime += Time.deltaTime; // update how much time is passed to signal moving passed warnings
            yield return null;
        }

        audioSrc.clip = laserFireAudio;
        audioSrc.pitch = laserFireAudio.length / activeDuration; // only play the sound for as long as activate duration is 
        audioSrc.Play();
        sprite.color = new Color(1,1,1,1); // brighten sprite to make it more vibrant
        laserCollider.enabled = true; // enable collider
        yield return new WaitForSeconds(activeDuration); // wait until the active duration is complete to move forward

        Destroy(gameObject); // destroy laser
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
