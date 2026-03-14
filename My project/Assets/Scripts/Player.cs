using UnityEngine;

public class Player : MonoBehaviour {
    // set in inspector
    public float speed = 5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    private GalacticChudInputs inputActions;    
    private const float Y_LIMIT = 4.6f;
    private const float X_LIMIT = 8.2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        inputActions = new();
        inputActions.Enable();
        inputActions.Standard.Enable();
    }

    // Update is called once per frame
    void Update(){
        // Player Shooting
        if (inputActions.Standard.Shoot.WasPressedThisFrame()){
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        }

        // Player Movement
        if (inputActions.Standard.MoveUp.IsPressed()){
            this.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (inputActions.Standard.MoveDown.IsPressed()){
            this.transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else if (inputActions.Standard.MoveRight.IsPressed())
        {
            this.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (inputActions.Standard.MoveLeft.IsPressed())
        {
            this.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Ensure player does not go off screen
        if (this.transform.position.y > Y_LIMIT)
        {
            this.transform.position = new Vector3(transform.position.x, Y_LIMIT);
        }
        else if (this.transform.position.y < -Y_LIMIT)
        {
            this.transform.position = new Vector3(transform.position.x, -Y_LIMIT);
        }
        else if (this.transform.position.x > X_LIMIT)
        {
            this.transform.position = new Vector3(X_LIMIT, transform.position.y);
        }
        else if (this.transform.position.x < -X_LIMIT)
        {
            this.transform.position = new Vector3(-X_LIMIT, transform.position.y);
        }
    }
}
