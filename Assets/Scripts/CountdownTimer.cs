﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 45f; // tiempo inicial en segundos, modificable desde el Inspector
    private float timeRemaining;

    public TMP_Text timerText; // arrastrar un Text de UI aquí desde el Canvas
    public GameObject generalPanel; // 🔹 ya no se usa para win/lose, lo dejamos por si lo necesitás
    public GameObject winPanel;     // 🔹 nuevo panel para victoria
    public GameObject losePanel;    // 🔹 nuevo panel para derrota

    public int unlockedSafes;

    private bool timerRunning = true;

    void Start()
    {
        unlockedSafes = 0;
        timeRemaining = startTime;
        UpdateTimerDisplay();

        // aseguramos que los paneles estén ocultos al inicio
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (generalPanel != null) generalPanel.SetActive(false);
    }

    void Update()
    {
        ResetGame();

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

        Time.timeScale = 0f;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    public void ResetGame()
    {
        if (Input.GetKeyDown(KeyCode.Space) && unlockedSafes >= 3 || Input.GetKeyDown(KeyCode.Space) && !timerRunning)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game");
        }
    }

    public void CheckWin()
    {
        if (unlockedSafes >= 3)
        {
            Time.timeScale = 0f;

            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }
        }
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
