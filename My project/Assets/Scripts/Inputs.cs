using UnityEngine;

public class Inputs : MonoBehaviour
{
    public static Inputs Instance { get; private set; }
    public GalacticChudInputs.StandardActions input;

    private void Awake(){
        Instance = this;
        var inputActions = new GalacticChudInputs();
        inputActions.Enable();
        input = inputActions.Standard;
        input.Enable();
    }
    
    public void DisableInput(){ // ensure no inputs can be made
        input.Disable();
    }

    public void EnableInput(){ // ensure input is given back
        input.Enable();
    }
}
