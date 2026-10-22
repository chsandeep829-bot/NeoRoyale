using UnityEngine;

/// <summary>
/// Applies lightweight quality defaults for a mobile-friendly prototype.
/// Set target frame rate and optional quality level in the Inspector.
/// </summary>
public class RuntimeQualitySettings : MonoBehaviour
{
    [Range(30, 120)] public int targetFrameRate = 60;
    [Range(0, 5)] public int qualityLevel = 2;
    public bool disableVSync = true;
    public bool keepScreenAwake = true;
    public bool applyOnStart = true;

    private void Start()
    {
        if (applyOnStart) ApplySettings();
    }

    public void ApplySettings()
    {
        Application.targetFrameRate = targetFrameRate;
        if (disableVSync) QualitySettings.vSyncCount = 0;
        QualitySettings.SetQualityLevel(
            Mathf.Clamp(qualityLevel, 0, QualitySettings.names.Length - 1), true);

        if (keepScreenAwake)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void SetTargetFrameRate(int fps)
    {
        targetFrameRate = Mathf.Clamp(fps, 30, 120);
        Application.targetFrameRate = targetFrameRate;
    }

    public void SetQualityLevel(int level)
    {
        qualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(qualityLevel, true);
    }
}
