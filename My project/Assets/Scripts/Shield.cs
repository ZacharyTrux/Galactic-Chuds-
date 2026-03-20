using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Shield : MonoBehaviour {
    // set in inspector
    public float maxProtectionTime;
    public GameObject shield;

    // private
    private Slider slider;
    private float protectionTime;
    private GalacticChudInputs.StandardActions input;

    public bool IsActive { get; private set; }

    void Start() {
        var inputActions = new GalacticChudInputs();
        inputActions.Enable();
        input = inputActions.Standard;
        input.Enable();

        slider = GetComponent<Slider>();
        slider.value = 1.0f;
        protectionTime = maxProtectionTime;
        shield.SetActive(false);
    }

    void Update() {
        slider.value = Mathf.Clamp(protectionTime / maxProtectionTime, 0f, 1f);
        if (input.Shield.IsPressed()) {
            if (protectionTime > 0) {
                protectionTime -= Time.deltaTime;
                IsActive = true;
            }
            else {
                IsActive = false;
                protectionTime = 0;
            }
        }
        else {
            protectionTime += Time.deltaTime;
            protectionTime = Mathf.Clamp(protectionTime, 0, maxProtectionTime);
            IsActive = false;
        }
        shield.SetActive(IsActive);
    }

    public void FullRefill() {
        protectionTime = maxProtectionTime;
    }
}
