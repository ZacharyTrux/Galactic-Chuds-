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

public enum GameState { Traversal, Boss, Menu }

public class GameManager : MonoBehaviour {
    public GameState currState = GameState.Menu;
    public float scoreToStartBoss = 7000;
    public float enemySpawnDelay;
    public float powerUpSpawnDelay;
    public PowerupType[] traversalPowerups;
    public PowerupType[] bossPowerups;

    public EnemyType[] enemies;
    public GameObject boss;
    public BoxCollider2D enemySpawnRange;
    public UI ui;
    public AudioClip bossTheme;
    public AudioClip traversalTheme;
    public AudioClip victoryTheme;

    private float enemySpawnTimer;
    private float powerUpSpawnTimer;
    private AudioSource audioSrc;

    public static GameManager Instance { get; set; }

    void Start() {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
        Instance = this;
    }

    void SpawnRandomEnemy() {
        if (enemySpawnRange == null) return;

        UnityEngine.Vector3 enemySpawnPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);

        int totalWeight = 0;
        foreach (var enemy in enemies) totalWeight += enemy.weight;

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

    void SpawnPowerUp(PowerupType[] pool) {
        if (pool.Length == 0 || enemySpawnRange == null) return;

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

    void HandleTraversalState() {
        if (Score.Instance.GetScore() >= scoreToStartBoss) {
            StartBossState();
            return;
        }

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnDelay) {
            SpawnRandomEnemy();
            enemySpawnTimer = 0.0f;
        }

        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            SpawnPowerUp(traversalPowerups);
            powerUpSpawnTimer = 0.0f;
        }
    }

    void StartBossState() {
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

        yield return new WaitForSeconds(victoryTheme.length);

        Score.Instance.UpdateScore(5000f);
        scoreToStartBoss += 15000f;
        enemySpawnDelay *= 0.8f;

        currState = GameState.Traversal;
        audioSrc.loop = true;
        audioSrc.clip = traversalTheme;
        audioSrc.Play();
    }
}
