using UnityEngine;

public class MatchTimer : MonoBehaviour
{
    [Min(1f)] public float matchDuration = 600f;
    public bool countDown = true;
    public float ElapsedSeconds { get; private set; }
    public float RemainingSeconds => Mathf.Max(0f, matchDuration - ElapsedSeconds);

    private void Update()
    {
        if (Time.timeScale <= 0f) return;
        ElapsedSeconds += Time.deltaTime;
        if (countDown && ElapsedSeconds >= matchDuration)
            ElapsedSeconds = matchDuration;
    }

    public string GetFormattedTime()
    {
        float seconds = countDown ? RemainingSeconds : ElapsedSeconds;
        int minutes = Mathf.FloorToInt(seconds / 60f);
        int remainder = Mathf.FloorToInt(seconds % 60f);
        return minutes.ToString("00") + ":" + remainder.ToString("00");
    }
}
