using UnityEngine;
using UnityEngine.UI;

public enum BossState {Shielded, Vulnerable}

public class BossFight : MonoBehaviour{
    public int missilesToBreak = 2;
    public float maxHealth = 1f;
    public Slider sliderHealth;
    public BossState currState = BossState.Shielded;
    public float attackCooldown = 4f;
    private BossLaser LaserScript;


    
    private float health = 0f;
    private int missileHits = 0;
    private float phaseHealth = 0.25f;
    private float laserTimer;

    public static BossFight Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        health = maxHealth;
        sliderHealth.value = health;
        Instance = this;

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

    public void takeDamage(float damage, string damageType){
        if(currState == BossState.Shielded && damageType == "Missile"){
            ShieldedPhaseDamage();
        }
        else if(currState == BossState.Vulnerable && damageType == "Bullet"){
            VulnerablePhaseDamage(damage);
        }
    }

    void VulnerablePhaseDamage(float damage){
        health -= damage;
        phaseHealth -= damage;
        
        if(phaseHealth <= 0){
            StartShieldedPhase();
        }
    }

    void ShieldedPhaseDamage(){
        missileHits += 1;
        
        if(missileHits >= missilesToBreak){
            StartVulnerablePhase();
        }
    }

    void StartVulnerablePhase(){
        currState = BossState.Vulnerable;
        phaseHealth = 0.25f;
    }

    void StartShieldedPhase(){
        currState = BossState.Shielded;
        missileHits = 0;
    }


}
