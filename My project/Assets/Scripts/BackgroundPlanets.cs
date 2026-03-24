using UnityEngine;

public class BackgroundPlanets : MonoBehaviour{
    // Public, inspector values 
    public Sprite[] planetSprites;
    public float speed = 1f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minScale = 1f;
    public float maxScale = 3.5f;

    // private values
    private SpriteRenderer[] childSprites;
    private Transform[] childTransforms;
    private float spawnPoint = 11;
    private float resetPoint = -20;
    private float[] currSpeeds;

    // upon start perform these actions
    void Awake(){ 
        // instantiate and retrieve private variables
        int childCount = transform.childCount;
        childTransforms = new Transform[childCount];
        childSprites = new SpriteRenderer[childCount];
        currSpeeds = new float[childCount];
        for(int i = 0; i < childCount; i++){
            childTransforms[i] = transform.GetChild(i);
            childSprites[i] = childTransforms[i].GetComponent<SpriteRenderer>();

            RandomizeAppearance(i);

            float startX = Random.Range(spawnPoint, resetPoint); // start the planets in a random position on the screen on game start
            childTransforms[i].localPosition = new Vector3(startX, childTransforms[i].localPosition.y, 0); // place planets
        }
    }

    // perform every frame 
    void Update(){ 
        for(int i = 0; i < childTransforms.Length; i++){
            childTransforms[i].Translate(Vector3.left * currSpeeds[i] * Time.deltaTime); // planet moves left on the screen

            if(childTransforms[i].localPosition.x < resetPoint){ // reset x position when the planet goes off screen
                childTransforms[i].localPosition = new Vector3(spawnPoint, Random.Range(minY, maxY), 0);
                RandomizeAppearance(i);
            }
        }
    }

    public void RandomizeAppearance(int index){
        childSprites[index].sprite = planetSprites[Random.Range(0, planetSprites.Length)]; // pick a random sprite from sprite list

        float randomScale = Random.Range(minScale, maxScale); // make the scale random
        childTransforms[index].localScale = new Vector3(randomScale, randomScale, 1f);

        currSpeeds[index] = (randomScale / maxScale) * speed; // change the speed based off scale (smaller, further, slower and larger, closer, faster)
    }
}