using UnityEngine;
using UnityEngine.UI; // para usar UI Text

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 45f; // tiempo inicial en segundos, modificable desde el Inspector
    private float timeRemaining;

    public Text timerText; // arrastrar un Text de UI aquí desde el Canvas

    private bool timerRunning = true;

    void Start()
    {
        timeRemaining = startTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            UpdateTimerDisplay();
            TimerFinished();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        Debug.Log("Tiempo terminado!");
        // Aquí podés poner lo que pasa cuando el timer llega a cero
    }

    // --- Opcional: reiniciar timer desde código ---
    public void ResetTimer(float newTime)
    {
        startTime = newTime;
        timeRemaining = startTime;
        timerRunning = true;
        UpdateTimerDisplay();
    }
}
