using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType{
        public GameObject prefab;
        public int weight;
    }
    
    public EnemyType[] enemyPrefabs;
    private float spawnRate = 1f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("GetRandomEnemy", 2f, spawnRate);
    }

    // Update is called once per frame
    void GetRandomEnemy(){
        if(Time.timeScale == 1f){ 
            int totalWeight = 0;
            foreach (var enemy in enemyPrefabs){
                totalWeight += enemy.weight;
            }

            int randNum = Random.Range(0, totalWeight);
            int weightSum = 0;

            foreach(var enemy in enemyPrefabs){
                weightSum += enemy.weight;
                if(randNum < weightSum){
                    Spawn(enemy.prefab);
                    break;
                }
            }    
        }
    }

    void Spawn(GameObject prefab){
        float spawnX = 10f;
        float spawnY = Random.Range(-5f, 5f);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
