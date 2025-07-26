using UnityEngine;

public class SlowMoToggle : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Z;
    public float slowMoTimeScale = 0.3f;
    public float slowMoFOV = 90f;
    public float normalTimeScale = 1f;
    public float normalFOV = 60f;
    public float fovTransitionSpeed = 5f;

    private bool isSlowMo = false;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("No Main Camera found. Make sure your main camera is tagged as 'MainCamera'.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleSlowMo();
        }

        // Smooth FOV transition
        float targetFOV = isSlowMo ? slowMoFOV : normalFOV;
        if (mainCam != null)
        {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, targetFOV, Time.unscaledDeltaTime * fovTransitionSpeed);
        }
    }

    void ToggleSlowMo()
    {
        isSlowMo = !isSlowMo;
        Time.timeScale = isSlowMo ? slowMoTimeScale : normalTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Keep physics stable
    }
}
