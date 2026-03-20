using UnityEngine;

[System.Serializable]
public class EnemyType{
    public GameObject prefab;
    public int weight;
}

public class GameManager : MonoBehaviour
{
    public float enemySpawnDelay;
    public float powerUpSpawnDelay;
    public GameObject powerUpPrefab;

    public EnemyType[] enemies;
    public BoxCollider2D enemySpawnRange;

    private float enemySpawnTimer;
    private float powerUpSpawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void SpawnRandomEnemy(){
        UnityEngine.Vector3 enemySpawnPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);

        int totalWeight = 0;
        foreach (var enemy in enemies){
            totalWeight += enemy.weight;
        }
        int randNum = Random.Range(0, totalWeight);
        int weightSum = 0;
        foreach(var enemy in enemies){
            weightSum += enemy.weight;
            if(randNum < weightSum){
                Instantiate(enemy.prefab, enemySpawnPt, UnityEngine.Quaternion.identity);
                break;
            }
        }    
    }

    void spawnPowerUp()
    {
        UnityEngine.Vector3 powerUpPt = new UnityEngine.Vector3(
            Random.Range(enemySpawnRange.bounds.min.x, enemySpawnRange.bounds.max.x),
            Random.Range(enemySpawnRange.bounds.min.y, enemySpawnRange.bounds.max.y),
            0);
        //Instantiate(powerUpPrefab, powerUpPt, UnityEngine.Quaternion.identity);
    }

    void Update(){
        enemySpawnTimer += Time.deltaTime;
        if(enemySpawnTimer >= enemySpawnDelay){
            SpawnRandomEnemy();
            enemySpawnTimer = 0.0f;
        }

        powerUpSpawnTimer += Time.deltaTime;
        if (powerUpSpawnTimer >= powerUpSpawnDelay) {
            spawnPowerUp();
            powerUpSpawnTimer = 0.0f;
        }
    }

    void GameOver(){
        
    }
}
