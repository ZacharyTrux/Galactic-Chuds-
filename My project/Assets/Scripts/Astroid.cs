using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class EnemyController : MonoBehaviour {
    // public, inspector values
    public float speed = 4f; 
    public float destroyAtX = -11f;
    public GameObject expoPrefab;
    public AudioClip explosionAudio;

    private AudioSource audioSrc;

    private void Update(){
        transform.Translate(Vector2.left * speed * Time.deltaTime); // move left every in game second

        if (transform.position.x < destroyAtX) { // eliminate object after going off screen
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Shield")) {
            Destroy(gameObject);
            Explosion();
        }
        if(other.gameObject.CompareTag("Missile")){
            Explosion();
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player Bullet")) {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player")){ // damage player 
            other.gameObject.GetComponentInParent<Player>().DamageFromEnemy();
            if(!other.gameObject.GetComponentInParent<Player>().IsInvincible()){
                Destroy(gameObject);
            }
        }
    }

    private void Explosion(){
        AudioSource.PlayClipAtPoint(explosionAudio, transform.position);
        var expoObj = Instantiate(expoPrefab, transform.position, UnityEngine.Quaternion.identity); // creates explosion of enemy object
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
    }
}
