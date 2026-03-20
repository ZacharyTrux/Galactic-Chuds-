using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    private GalacticChudInputs.StandardActions input;

    void Start()
    {
        var inputActions = new GalacticChudInputs();
        inputActions.Enable();
        input = inputActions.Standard;
        input.Enable();

        container.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Pause.WasPressedThisFrame())
        {
            container.SetActive(true);
            Time.timeScale = 0f;
        } 
        */
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
