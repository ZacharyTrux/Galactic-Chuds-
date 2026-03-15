using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        if(container != null)
        {
            container.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.inputActions.Standard.Pause.IsPressed())
        {
            container.SetActive(true);
            Time.timeScale = 0f;
        } 
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1f;
    }
    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
