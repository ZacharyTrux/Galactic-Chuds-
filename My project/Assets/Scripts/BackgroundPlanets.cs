using UnityEngine;

public class BackgroundPlanets : MonoBehaviour{
    public Sprite[] planetSprites;
    public int spawnX = 12;
    public float speed = 1f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minScale = 1f;
    public float maxScale = 3.5f;

    private SpriteRenderer[] childSprites;
    private Transform[] childTransforms;
    private float spawnPoint = 11;
    private float resetPoint = -20;
    private float[] currSpeeds;

    void Awake(){
        int childCount = transform.childCount;
        childTransforms = new Transform[childCount];
        childSprites = new SpriteRenderer[childCount];
        currSpeeds = new float[childCount];
        for(int i = 0; i < childCount; i++){
            childTransforms[i] = transform.GetChild(i);
            childSprites[i] = childTransforms[i].GetComponent<SpriteRenderer>();

            RandomizeAppearance(i);

            float startX = Random.Range(spawnPoint, resetPoint);
            childTransforms[i].localPosition = new Vector3(startX, childTransforms[i].localPosition.y, 0);
        }
    }

    void Update(){
        for(int i = 0; i < childTransforms.Length; i++){
            childTransforms[i].Translate(Vector3.left * currSpeeds[i] * Time.deltaTime);

            if(childTransforms[i].localPosition.x < resetPoint){
                childTransforms[i].localPosition = new Vector3(spawnPoint, Random.Range(minY, maxY), 0);
                RandomizeAppearance(i);
            }
        }
    }

    public void RandomizeAppearance(int index){
        childSprites[index].sprite = planetSprites[Random.Range(0, planetSprites.Length)];

        float randomScale = Random.Range(minScale, maxScale);
        childTransforms[index].localScale = new Vector3(randomScale, randomScale, 1f);

        currSpeeds[index] = (randomScale / maxScale) * speed;
    }
}