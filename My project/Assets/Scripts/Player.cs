using UnityEngine.UI;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;



public class Player : MonoBehaviour {
    // set in inspector
    public float speed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Slider sliderHealth;
    public Shield shield;   
    public UI ui;
    //public AudioSource shoot;
    //public AudioSource damage;



    private Inputs input;    
    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 8.2f;
    private float health;
    //private AudioSource audioSrc;
    
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        health = 1.0f;
    }

    // Update is called once per frame
    void Update(){
        sliderHealth.value = health;

        // Player Shooting
        if (Inputs.Instance.input.Shoot.WasPressedThisFrame()){
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, UnityEngine.Quaternion.identity);
            //shootingSound.Play();
        }

        // Player Movement
        var vertMove = Inputs.Instance.input.MoveVertically.ReadValue<float>();
        var horizontalMove = Inputs.Instance.input.MoveHortizontally.ReadValue<float>();
        this.transform.Translate(UnityEngine.Vector3.up * speed * Time.deltaTime * vertMove);
        this.transform.Translate(UnityEngine.Vector3.right * speed * Time.deltaTime * horizontalMove);
        CheckBounds();
        // Ensure player does not go off screen
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
        if (!shield.IsActive) {
            health -= 0.25f;
        }

        if(health <= 0){
            ui.ShowGameOver();
        }
        //audioSrc.clip = damage;
        //audioSrc.Play();
    }

    public void RefillShield() {
        shield.FullRefill();
        
    }
}
