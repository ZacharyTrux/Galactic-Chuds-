using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject health;
    public GameObject score;

    private static bool SkipTitle = false;

    public bool IsReady { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        gameOver.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if(SkipTitle){
            StartGame();
            SkipTitle = false;
        }
        else{
            health.SetActive(false);
            score.SetActive(false);
            titleScreen.SetActive(true);
            Inputs.Instance.DisableInput();
            IsReady = false;
        }
    }

    public void ShowGameOver(){
        gameOver.SetActive(true);

        TMPro.TextMeshProUGUI scoreText = gameOver.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        scoreText.text = "Final Score: " + Score.Instance.GetScore();

        Inputs.Instance.DisableInput();
        IsReady = false;
    }

    public void PauseGame(){
        Inputs.Instance.DisableInput();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame(){
        Inputs.Instance.EnableInput();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnMainMenu(){
        SceneManager.LoadScene(0);
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void StartGame(){
        health.SetActive(true);
        score.SetActive(true);
        Inputs.Instance.EnableInput();
        titleScreen.SetActive(false);
        GameManager.Instance.currState = GameState.Traversal;
        IsReady = true;
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();
    }
}
