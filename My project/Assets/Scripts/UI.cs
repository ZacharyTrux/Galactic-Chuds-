using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOver;
    public GameObject pauseMenu;

    public bool IsReady { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        gameOver.SetActive(false);
        titleScreen.SetActive(true);
        Inputs.Instance.DisableInput();
        IsReady = false;
    }

    public void ShowGameOver(){
        gameOver.SetActive(true);
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
        pauseMenu.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void StartGame(){
        Inputs.Instance.EnableInput();
        titleScreen.SetActive(false);
        IsReady = true;
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void Quit(){
        Application.Quit();
    }
}
