using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOver;
    public GameObject 

    public bool IsReady { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        gameOver.SetActive(false);
        titleScreen.SetActive(true);
        SpaceShooterInput.Instance.DisableInput();
        IsReady = false;
    }

    public void ShowGameOver(){
        gameOver.SetActive(true);
        GalacticChudInputs.Instance.DisableInput();
        IsReady = false;

    }

    public void StartGame(){
        GalacticChudInputs.Instance.EnableInput();
        titleScreen.SetActive(false);
        IsReady = true;
    }

    public void RestartGame(){
        SceneManager.LoadScene(1);
    }
}
