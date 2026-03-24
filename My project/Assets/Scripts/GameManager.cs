using UnityEngine;

[System.Serializable]
public class EnemyType {
    public GameObject prefab;
    public int weight;
}

<<<<<<< Updated upstream
[System.Serializable]
public class PowerupType {
    public GameObject prefab;
    public int weight;
}

public enum GameState { Traversal, Boss, Menu }

public class GameManager : MonoBehaviour {
    public GameState currState = GameState.Menu;
    public float scoreToStartBoss = 7000;
    public float enemySpawnDelay;
    public float powerUpSpawnDelay;
    public PowerupType[] traversalPowerups;
    public PowerupType[] bossPowerups;

=======
public enum GameState {Traversal, Boss, Menu} // valid GameStates

public class GameManager : MonoBehaviour{
    // public, inspector variables
    public float scoreToStartBoss = 7000;
    public float enemySpawnDelay;
    public float powerUpSpawnDelay;
    public GameState currState = GameState.Menu;
    public GameObject powerUpPrefab;
>>>>>>> Stashed changes
    public EnemyType[] enemies;
    public GameObject boss;
    public BoxCollider2D enemySpawnRange;
    public UI ui;
    public AudioClip bossTheme;
    public AudioClip traversalTheme;
    public AudioClip victoryTheme;
    public static GameManager Instance { get; private set; }

    // private varirables
    private float enemySpawnTimer;
    private float powerUpSpawnTimer;
    private AudioSource audioSrc;

    

<<<<<<< Updated upstream
    void Start() {
=======
    void Start(){ // initialize audio components and instance
>>>>>>> Stashed changes
        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
        Instance = this;
    }

<<<<<<< Updated upstream
    void SpawnRandomEnemy() {
        if (enemySpawnRange == null) return;

=======
    void SpawnRandomEnemy(){ // spawn random enemy within the box collider
>>>>>>> Stashed changes
        UnityEngine.Vector3 enemySpawnPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);

<<<<<<< Updated upstream
        int totalWeight = 0;
        foreach (var enemy in enemies) totalWeight += enemy.weight;

=======
        int totalWeight = 0; // assigned weights to enemies determine which ones to spawn
        foreach (var enemy in enemies){
            totalWeight += enemy.weight;
        }
>>>>>>> Stashed changes
        int randNum = Random.Range(0, totalWeight);
        int weightSum = 0;
        foreach (var enemy in enemies) {
            weightSum += enemy.weight;
            if (randNum < weightSum) {
                Instantiate(enemy.prefab, enemySpawnPt, UnityEngine.Quaternion.identity);
                break;
            }
        }
    }

<<<<<<< Updated upstream
    void SpawnPowerUp(PowerupType[] pool) {
        if (pool.Length == 0 || enemySpawnRange == null) return;

=======
    void spawnPowerUp(){ // spawn random powerup within the box collider
>>>>>>> Stashed changes
        UnityEngine.Vector3 powerUpPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);

        int totalWeight = 0;
        foreach (var p in pool) totalWeight += p.weight;

        int randNum = Random.Range(0, totalWeight);
        int weightSum = 0;
        foreach (var p in pool) {
            weightSum += p.weight;
            if (randNum < weightSum) {
                Instantiate(p.prefab, powerUpPt, UnityEngine.Quaternion.identity);
                break;
            }
        }
    }

<<<<<<< Updated upstream
    void ClearTraversalEnemies() {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);
    }

    void Update() {
        if (Inputs.Instance.input.Pause.WasPressedThisFrame()) {
            ui.PauseGame();
        }

        if (currState == GameState.Menu) return;

        if (currState == GameState.Traversal) {
            HandleTraversalState();
        }
        else if (currState == GameState.Boss) {
=======
    void Update(){
        if(Inputs.Instance.input.Pause.WasPressedThisFrame()){ // pause game if necessary
            ui.PauseGame();
        }

        if(currState == GameState.Traversal){ // spawning enemies 
            HandleTraversalState();
        }
        
        else if(currState == GameState.Boss){ // boss encounter
>>>>>>> Stashed changes
            HandleBossState();
        }
    }

<<<<<<< Updated upstream
    void HandleTraversalState() {
        if (Score.Instance.GetScore() >= scoreToStartBoss) {
=======
    void HandleTraversalState(){
        if(Score.Instance.GetScore() >= scoreToStartBoss){ // if the player exceeds score, then spawn boss and exit
>>>>>>> Stashed changes
            StartBossState();
            return;
        }

        enemySpawnTimer += Time.deltaTime;
<<<<<<< Updated upstream
        if (enemySpawnTimer >= enemySpawnDelay) {
=======
        if(enemySpawnTimer >= enemySpawnDelay){ // spawn random enemy when timer goes off
>>>>>>> Stashed changes
            SpawnRandomEnemy();
            enemySpawnTimer = 0.0f;
        }

        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            SpawnPowerUp(traversalPowerups);
            powerUpSpawnTimer = 0.0f;
        }
    }

<<<<<<< Updated upstream
    void StartBossState() {
=======
    void StartBossState(){ // play boss theme and create the boss 
>>>>>>> Stashed changes
        currState = GameState.Boss;
        ClearTraversalEnemies();
        Instantiate(boss, enemySpawnRange.bounds.center, UnityEngine.Quaternion.identity);
        audioSrc.Stop();
        audioSrc.clip = bossTheme;
        audioSrc.Play();
    }

<<<<<<< Updated upstream
    void HandleBossState() {
        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            SpawnPowerUp(bossPowerups);
            powerUpSpawnTimer = 0.0f;
        }

        if (GameObject.FindGameObjectWithTag("Boss") == null) {
=======
    void HandleBossState(){ // boss is dead, do nothing except victory audio
        if(GameObject.FindGameObjectWithTag("Boss") == null){
>>>>>>> Stashed changes
            currState = GameState.Menu;
            StartCoroutine(PlayVictory());
        }
    }

    System.Collections.IEnumerator PlayVictory() {
        audioSrc.Stop();
        audioSrc.clip = victoryTheme;
        audioSrc.loop = false;
        audioSrc.Play();

        yield return new WaitForSeconds(victoryTheme.length); // song plays out

        // update values to make entities spawn faster and next boss phase takes longer
        Score.Instance.UpdateScore(5000f);
        scoreToStartBoss += 15000f;
<<<<<<< Updated upstream
        enemySpawnDelay *= 0.8f;
=======
        enemySpawnDelay = System.Math.Max(0.5f, (enemySpawnDelay * 0.8f)); 
>>>>>>> Stashed changes


        // return to traversal 
        currState = GameState.Traversal;
        audioSrc.loop = true;
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
    }
}
