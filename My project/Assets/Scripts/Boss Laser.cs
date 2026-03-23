using UnityEngine;
using System.Collections.Generic;

public class BossLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform LocationContainer;
    public int totalSafeZones = 2;

    private BoxCollider2D[] spawnLocations;

    void Start(){
        spawnLocations = LocationContainer.GetComponentsInChildren<BoxCollider2D>();
    }

    public void FirePattern(){
        List<int> indices = new List<int>();
        for(int i = 0; i < spawnLocations.Length; i++){
            indices.Add(i);
        }

        for(int i = 0; i < indices.Count; i++){
            int temp = indices[i];
            int randomIndex = Random.Range(i, indices.Count);
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        for(int i = totalSafeZones; i < indices.Count; i++){
            Vector2 spawnPos = spawnLocations[indices[i]].transform.position;
            Instantiate(laserPrefab, spawnPos, Quaternion.Euler(0,0,180));
        }
    }
}
