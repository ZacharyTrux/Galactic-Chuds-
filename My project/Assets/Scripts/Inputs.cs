using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Inputs Instance { get; private set; }
    public GalacticChudInputs.StandardActions input;

    private void Awake()
    {
        Instance = this;
        var inputActions = new GalacticChudInputs();
        inputActions.Enable();
        input = inputActions.Standard;
        input.Enable();
    }
    
    public void DisableInput(){
        input.Disable();
    }

    // Update is called once per frame
    public void EnableInput(){
        input.Enable();
    }
}
