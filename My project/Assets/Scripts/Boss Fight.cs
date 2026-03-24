using UnityEngine;
using UnityEngine.UI;

public enum BossState {Shielded, Vulnerable}

public class BossFight : MonoBehaviour{
    public int missilesToBreak = 2;
    public float maxHealth = 1f;
    public Slider sliderHealth;
    public BossState currState = BossState.Shielded;
    public float attackCooldown = 4f;
    public GameObject expoPrefab;
    public AudioClip expoAudio;
    public GameObject[] bossHeads;

    
    private float health = 0f;
    private int missileHits = 0;
    private float phaseHealth = 0.25f;
    private float laserTimer;
    private int numExplosions = 5;
    private int currHead;
    private BossLaser LaserScript;
    private AudioSource audioSrc;

    public static BossFight Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        health = maxHealth;
        sliderHealth.value = health;
        Instance = this;
        audioSrc = GetComponent<AudioSource>();

        LaserScript = GetComponent<BossLaser>();
    }

    void Update(){
        sliderHealth.value = health;

        if(currState == BossState.Shielded){
            HandleAttacking();
        }
    }

    void HandleAttacking(){
        laserTimer += Time.deltaTime;
        if(laserTimer >= attackCooldown){
            LaserScript.FirePattern();
            laserTimer = 0;
        }
    }

    void VulnerablePhaseDamage(float damage){
        health -= damage;
        phaseHealth -= damage;
        
        if(health <= 0){
            BossDeath();
        }
        else if(phaseHealth <= 0){
            StartShieldedPhase();
        }
    }

    void ShieldedPhaseDamage(){
        missileHits += 1;
        int index = System.Math.Abs((missileHits / missilesToBreak) - 1);
        ChangeHead(index);

        if(missileHits >= missilesToBreak){
            StartVulnerablePhase();
        }
    }

    void StartVulnerablePhase(){
        currState = BossState.Vulnerable;
        phaseHealth = 0.25f;
    }

    void ChangeHead(int index){
        if(index == 2){
            bossHeads[currHead].SetActive(false);
            return;
        }
        bossHeads[currHead].SetActive(false);
        bossHeads[index].SetActive(true);
        currHead = index;
    }

    void StartShieldedPhase(){
        ChangeHead(2);
        currState = BossState.Shielded;
        missileHits = 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player Bullet") && currState == BossState.Vulnerable){
            VulnerablePhaseDamage(0.02f);
            Destroy(other.gameObject);
        }
        else if(other.TryGetComponent<Missile>(out Missile missile)){
            audioSrc.clip = expoAudio;
            audioSrc.Play();
            var expoObj = Instantiate(expoPrefab, transform.position, UnityEngine.Quaternion.identity); // creates explosion of enemy object
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
            Destroy(other.gameObject);
            ShieldedPhaseDamage();
        }
        else if(other.CompareTag("Shield")){
        }
        else if(other.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().DamageFromEnemy();
        }
    }

    void BossDeath(){
        StartCoroutine(DeathAnimation());
    }

    System.Collections.IEnumerator DeathAnimation(){
        PolygonCollider2D bossCollider = GetComponent<PolygonCollider2D>();
        bossCollider.enabled = false;

        for(int i = 0; i < numExplosions; i++){
            audioSrc.Play();
            Vector3 randomPosition = new Vector3(Random.Range(bossCollider.bounds.min.x, bossCollider.bounds.max.x), Random.Range(bossCollider.bounds.min.y, bossCollider.bounds.max.y));
            var expoObj = Instantiate(expoPrefab, randomPosition, UnityEngine.Quaternion.identity); // creates explosion of enemy object

            float randomScale = Random.Range(0.8f, 2.0f);
            expoObj.transform.localScale = Vector3.one * randomScale;

            yield return new WaitForSeconds(0.5f);
            Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
        }
        Destroy(gameObject);
    }
}
