using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {
    // public, inspector variables
    public GameObject titleScreen;
    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject health;
    public GameObject score;
    public bool IsReady { get; private set; }

    // private variables
    private static bool SkipTitle = false;

    // When starting the game, turn off UI elements and determine if title needs to be shown
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

    public void ShowGameOver(){ // Game Over UI
        gameOver.SetActive(true);
        TMPro.TextMeshProUGUI scoreText = gameOver.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        scoreText.text = "Final Score: " + Score.Instance.GetScore(); // show final score

        Inputs.Instance.DisableInput(); // ensure player can not perform inputs
        IsReady = false;
    }

    public void PauseGame(){ // pause the game in its current state
        Inputs.Instance.DisableInput();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame(){ // return game to current state
        Inputs.Instance.EnableInput();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReturnMainMenu(){ // Turn off all other UI besides Title Screen
        SceneManager.LoadScene(0);
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        titleScreen.SetActive(true);
        IsReady = false;
    }

    public void StartGame(){ // begin game and turn back on UI elements needed
        health.SetActive(true);
        score.SetActive(true);
        Inputs.Instance.EnableInput();
        titleScreen.SetActive(false);
        GameManager.Instance.currState = GameState.Traversal;
        IsReady = true;
    }

    public void RestartGame(){ // load scene back up, ignroing title screen
        SkipTitle = true;
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();
    }
}
