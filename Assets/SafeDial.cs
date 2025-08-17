using UnityEngine;

public class SafeDial : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource correctSound;
    [SerializeField] private AudioSource confirmSound;
    private int dialPosition = 0;       // 0..89
    private int[] combination = new int[3];
    private int currentStage = 0;       // 0–2
    private bool stageUnlocked = false;

    private float stepSize = 4f;        // 360° / 90 posiciones

    void Start()
    {
        GenerateCombination();
        Debug.Log($"Combinación: {combination[0]} - {combination[1]} - {combination[2]}");
    }

    void Update()
    {
        HandleInput();
        CheckInputConfirm();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            dialPosition = (dialPosition + 1) % 90;
            PlayStepSound();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            dialPosition = (dialPosition - 1 + 90) % 90;
            PlayStepSound();
        }

        // Rotación visual
        transform.localRotation = Quaternion.Euler(0f, 0f, -dialPosition * stepSize);
    }

    void PlayStepSound()
    {
        if (dialPosition == combination[currentStage])
        {
            correctSound.Play();
            stageUnlocked = true;
        }
        else
        {
            clickSound.Play();
            stageUnlocked = false;
        }
    }

    void CheckInputConfirm()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stageUnlocked)
            {
                confirmSound.Play(); // sonido de la barra solo si es correcto
                Debug.Log($"¡Número correcto en etapa {currentStage + 1}!");
                currentStage++;

                if (currentStage >= 3)
                    Debug.Log("¡Ganaste! Caja fuerte abierta.");
                else
                    stageUnlocked = false;
            }
            else
            {
                Debug.Log("No estás en el número correcto. Seguí buscando.");
            }
        }
    }


    void GenerateCombination()
    {
        for (int i = 0; i < 3; i++)
            combination[i] = Random.Range(0, 90);
    }
}
