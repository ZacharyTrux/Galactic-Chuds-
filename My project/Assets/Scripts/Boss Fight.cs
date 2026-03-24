using UnityEngine;
using UnityEngine.UI;

public enum BossState {Shielded, Vulnerable} // valid game states

public class BossFight : MonoBehaviour{
    // public, inspector variables 
    public int missilesToBreak = 2;
    public float maxHealth = 1f;
    public float attackCooldown = 4f;
    public GameObject expoPrefab;
    public AudioClip expoAudio;
    public AudioClip bossDamagedAudio;
    public GameObject[] bossHeads;
    public Slider sliderHealth;

    // private variables
    private float health = 0f;
    private int missileHits = 0;
    private float phaseHealth = 0.25f;
    private float laserTimer;
    private int numExplosions = 10;
    private int currHead;
    private BossLaser LaserScript;
    private BossState currState = BossState.Shielded;
    private AudioSource audioSrc;

    // Boss Instance
    public static BossFight Instance { get; private set; }

    
    // instantiate values;
    void Start(){
        health = maxHealth;
        sliderHealth.value = health;
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
        LaserScript = GetComponent<BossLaser>();
    }

    void Update(){ // update health bar every frame
        sliderHealth.value = health;

        if(currState == BossState.Shielded){
            HandleAttacking();
        }
    }

    void HandleAttacking(){ // attack when after cooldown finishes
        laserTimer += Time.deltaTime;
        if(laserTimer >= attackCooldown){
            LaserScript.FirePattern();
            laserTimer = 0;
        }
    }

    void VulnerablePhaseDamage(float damage){ // determine if phase has completed or boss health is below zero
        audioSrc.clip = bossDamagedAudio;
        audioSrc.Play();
        health -= damage;
        phaseHealth -= damage;
        
        if(health <= 0){
            BossDeath();
        }
        else if(phaseHealth <= 0){
            StartShieldedPhase();
        }
    }

    void ShieldedPhaseDamage(){ // change heads on boss based off missle hits
        missileHits += 1;
        int index = System.Math.Abs((missileHits / missilesToBreak) - 1);
        ChangeHead(index);

        if(missileHits >= missilesToBreak){
            StartVulnerablePhase();
        }
    }

    void StartVulnerablePhase(){ // transition to vulnerable phase
        currState = BossState.Vulnerable;
        phaseHealth = 0.25f;
    }

    void ChangeHead(int index){ // changing head logic
        if(index == 2){
            bossHeads[currHead].SetActive(false);
            return;
        }
        bossHeads[currHead].SetActive(false);
        bossHeads[index].SetActive(true);
        currHead = index;
    }

    void StartShieldedPhase(){ // reset heads and go back to shielded
        ChangeHead(2);
        currState = BossState.Shielded;
        missileHits = 0;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player Bullet") && currState == BossState.Vulnerable){ // missile or bullet will damage boss if in vulnerable mode
            VulnerablePhaseDamage(0.02f);
            Destroy(other.gameObject);
        }
        else if(other.TryGetComponent<Missile>(out Missile missile)){ // missile damage check for shield phase
            audioSrc.clip = expoAudio;
            audioSrc.Play();
            var expoObj = Instantiate(expoPrefab, transform.position, UnityEngine.Quaternion.identity); // creates explosion of enemy object
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
            Destroy(other.gameObject);
            ShieldedPhaseDamage();
        }
        else if(other.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
        }
    }

    void BossDeath(){ // go through death animation
        StartCoroutine(DeathAnimation());
    }

    System.Collections.IEnumerator DeathAnimation(){ // spawn a lot of explosions 
        audioSrc.clip = expoAudio;
        PolygonCollider2D bossCollider = GetComponent<PolygonCollider2D>();
        bossCollider.enabled = false;

        for(int i = 0; i < numExplosions; i++){
            audioSrc.Play();
            Vector3 randomPosition = new Vector3(Random.Range(bossCollider.bounds.min.x, bossCollider.bounds.max.x), Random.Range(bossCollider.bounds.min.y, bossCollider.bounds.max.y)); // place explosion in a random position on boss
            var expoObj = Instantiate(expoPrefab, randomPosition, UnityEngine.Quaternion.identity); // creates explosion of enemy object
            float randomScale = Random.Range(0.8f, 2.0f); // randomize scale of explosion
            expoObj.transform.localScale = Vector3.one * randomScale;

            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
            yield return new WaitForSeconds(0.5f); // wait 0.5 seconds for the next one
        }
        Destroy(gameObject); // destroy boss
    }
}
