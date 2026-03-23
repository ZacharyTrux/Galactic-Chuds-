using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI txtScore;
    private float score;
    public static Score instance { get; private set; }

    // retrieve the text box containing score text box
    void Awake()
    {
        txtScore = GetComponentInChildren<TextMeshProUGUI>();
        score = 0.0f;
        instance = this;
    }

    // Update is the UI every frame
    void Update()
    {
        txtScore.text = ((int)score).ToString("D8");
    }

    // Update the score based on enemy killed (newScore)
    public void UpdateScore(float newScore){
        score += newScore;
    }

    public float getScore(){
        return score;
    }
}
