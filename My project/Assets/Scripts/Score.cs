using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; } // instance for other scripts to access

    private TextMeshProUGUI txtScore;
    private float score;

    // retrieve the text box containing score text box
    void Awake(){
        txtScore = GetComponentInChildren<TextMeshProUGUI>();
        score = 0.0f;
        Instance = this;
    }

    // Update is the UI every frame
    void Update(){
        txtScore.text = ((int)score).ToString("D8");
    }

    // Update the score based on enemy killed (newScore)
    public void UpdateScore(float newScore){
        score += newScore;
    }

    public float GetScore(){
        return score;
    }
}
