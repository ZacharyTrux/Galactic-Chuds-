using System.Numerics;
using UnityEngine.UI;
using UnityEngine;



public class Player : MonoBehaviour {
    // set in inspector
    public float speed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Slider sliderHealth;


    private GalacticChudInputs.StandardActions input;    
    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 8.2f;
    private float health;
    
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        var inputActions = new GalacticChudInputs();
        inputActions.Enable();
        input = inputActions.Standard;
        input.Enable();
        health = 1.0f;
    }

    // Update is called once per frame
    void Update(){
        sliderHealth.value = health;
        // Player Shooting
        if (input.Shoot.WasPressedThisFrame()){
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, UnityEngine.Quaternion.identity);
        }

        // Player Movement
        var vertMove = input.MoveVertically.ReadValue<float>();
        var horizontalMove = input.MoveHortizontally.ReadValue<float>();
        this.transform.Translate(UnityEngine.Vector3.up * speed * Time.deltaTime * vertMove);
        this.transform.Translate(UnityEngine.Vector3.right * speed * Time.deltaTime * horizontalMove);

        // Ensure player does not go off screen
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
        health -= 0.25f;
    }
}
