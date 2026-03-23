using UnityEngine.UI;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour {
    // set in inspector
    public float iFrames = 3f;
    public float speed = 5f;
    public GameObject bulletPrefab;
    public GameObject misslePrefab;
    public Transform bulletSpawnPoint;
    public Slider sliderHealth;
    public Shield shield;   
    public UI ui;
    public GameObject expoPrefab;
    public AudioClip shootingSound;
    public AudioClip missleFireSound;
    public AudioClip damage;



    private Inputs input;    
    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 8.2f;
    private float health;
    private bool isInvincible = false;
    private SpriteRenderer sprite;
    
    private AudioSource audioSrc;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        health = 1.0f;
        audioSrc = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        sliderHealth.value = health;

        // Player Shooting
        if (Inputs.Instance.input.Shoot.WasPressedThisFrame()){
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, UnityEngine.Quaternion.identity);
            audioSrc.clip = shootingSound;
            audioSrc.Play();
        }
        else if(Inputs.Instance.input.Missle.WasPressedThisFrame()){
            GameObject missleObj = Instantiate(misslePrefab, bulletSpawnPoint.position, UnityEngine.Quaternion.identity);
            audioSrc.clip = missleFireSound;
            audioSrc.Play();
        }

        // Player Movement
        var vertMove = Inputs.Instance.input.MoveVertically.ReadValue<float>();
        var horizontalMove = Inputs.Instance.input.MoveHortizontally.ReadValue<float>();
        this.transform.Translate(UnityEngine.Vector3.up * speed * Time.deltaTime * vertMove);
        this.transform.Translate(UnityEngine.Vector3.right * speed * Time.deltaTime * horizontalMove);
        CheckBounds(); // Ensure player does not go off screen
    }

    private void CheckBounds(){
        if (this.transform.position.y > Y_LIMIT)
        {
            this.transform.position = new UnityEngine.Vector3(transform.position.x, Y_LIMIT);
        }
        else if (this.transform.position.y < -Y_LIMIT)
        {
            this.transform.position = new UnityEngine.Vector3(transform.position.x, -Y_LIMIT);
        }
        else if (this.transform.position.x > X_LIMIT)
        {
            this.transform.position = new UnityEngine.Vector3(X_LIMIT, transform.position.y);
        }
        else if (this.transform.position.x < -X_LIMIT)
        {
            this.transform.position = new UnityEngine.Vector3(-X_LIMIT, transform.position.y);
        }
    }

    public void DamageFromEnemy(){
        if(isInvincible) return;
        /*
        if (!shield.IsActive) {
            health -= 0.25f;
        }
        */
        health -= 0.25f;
        StartCoroutine(IFramesEnabled());

        if(health <= 0){
            sliderHealth.value = health;
            GameOver();
            ui.ShowGameOver();
        }
        audioSrc.clip = damage;
        audioSrc.Play();
    }

    public void RefillShield() {
        shield.FullRefill();
    }

    private System.Collections.IEnumerator IFramesEnabled(){
        isInvincible = true;
        for(int i = 0; i < 4; i++){
            sprite.color = new Color(1,1,1, 0.2f);
            yield return new WaitForSeconds(iFrames / 8);
            sprite.color = new Color(1,1,1, 1);
            yield return new WaitForSeconds(iFrames / 8);
        }

        isInvincible = false;
    }

    private void GameOver(){
        Destroy(gameObject);
        var expoObj = Instantiate(expoPrefab, transform.position, UnityEngine.Quaternion.identity); // creates explosion of enemy object
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration); // delete explosion after it goes off
    }

}
