using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI txtScore;
    private float score;
    public static Score Instance { get; private set; }

    // retrieve the text box containing score text box
    void Start()
    {
        txtScore = GetComponentInChildren<TextMeshProUGUI>();
        score = 0.0f;
        Instance = this;
    }

    // Update is the UI every frame
    void Update()
    {
        txtScore.text = score.ToString();
    }

    // Update the score based on enemy killed (newScore)
    public void UpdateScore(float newScore){
        score += newScore;
    }
}
