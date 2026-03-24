using UnityEngine;

[System.Serializable]
public class EnemyType {
    public GameObject prefab;
    public int weight;
}

[System.Serializable]
public class PowerupType {
    public GameObject prefab;
    public int weight;
}

public enum GameState {Traversal, Boss, Menu} // valid GameStates

public class GameManager : MonoBehaviour{
    // public, inspector variables
    public float scoreToStartBoss = 7000;
    public float enemySpawnDelay;
    public float powerUpSpawnDelay;
    public static bool SkipTitleNextLoad;
    public PowerupType[] traversalPowerups;
    public PowerupType[] bossPowerups;
    public EnemyType[] enemies;
    public GameObject boss;
    public BoxCollider2D enemySpawnRange;
    public UI ui;
    public AudioClip bossTheme;
    public AudioClip traversalTheme;
    public AudioClip victoryTheme;
    public GameState currState = GameState.Menu;
    public static GameManager Instance { get; private set; }

    // private varirables
    private float enemySpawnTimer;
    private float powerUpSpawnTimer;
    private AudioSource audioSrc;

    

    void Start(){ // initialize audio components and instance
        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
        Instance = this;
        if(SkipTitleNextLoad){
            currState = GameState.Traversal;
            SkipTitleNextLoad = false;
        }
        else{
            currState = GameState.Menu;
        }
    }

    void SpawnRandomEnemy(){ // spawn random enemy within the box collider
        UnityEngine.Vector3 enemySpawnPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);

        int totalWeight = 0; // assigned weights to enemies determine which ones to spawn
        foreach (var enemy in enemies){
            totalWeight += enemy.weight;
        }
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

    void SpawnPowerUp(PowerupType[] pool){ // spawn random powerup within the box collider
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
            HandleBossState();
        }
    }

    void HandleTraversalState(){
        if(Score.Instance.GetScore() >= scoreToStartBoss){ // if the player exceeds score, then spawn boss and exit
            StartBossState();
            return;
        }

        enemySpawnTimer += Time.deltaTime;
        if(enemySpawnTimer >= enemySpawnDelay){ // spawn random enemy when timer goes off
            SpawnRandomEnemy();
            enemySpawnTimer = 0.0f;
        }

        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            SpawnPowerUp(traversalPowerups);
            powerUpSpawnTimer = 0.0f;
        }
    }

    void StartBossState(){ // play boss theme and create the boss 
        currState = GameState.Boss;
        ClearTraversalEnemies();
        Instantiate(boss, enemySpawnRange.bounds.center, UnityEngine.Quaternion.identity);
        audioSrc.Stop();
        audioSrc.clip = bossTheme;
        audioSrc.Play();
    }

    void HandleBossState() {
        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            SpawnPowerUp(bossPowerups);
            powerUpSpawnTimer = 0.0f;
        }

        if (GameObject.FindGameObjectWithTag("Boss") == null) {
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
        enemySpawnDelay = System.Math.Max(0.5f, (enemySpawnDelay * 0.8f)); 


        // return to traversal 
        currState = GameState.Traversal;
        audioSrc.loop = true;
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
    }
}
