using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentDial;
    [SerializeField] private SafeDial[] totalOfDials;
    private SafeDial currentSafe;

    private float stepSize = 4f;        // 360° / 90 posiciones
    [SerializeField] private int countSafe = 1;

    private void Update()
    {
        MovePlayerToDials();
        HandleInput();
        CheckInputConfirm();
    }

    void HandleInput()
    {
        if (currentSafe != null)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentSafe.dialPosition = (currentSafe.dialPosition + 1) % 90;
                currentSafe.PlayStepSound();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentSafe.dialPosition = (currentSafe.dialPosition - 1 + 90) % 90;
                currentSafe.PlayStepSound();
            }

            // Rotación visual
            currentSafe.transform.localRotation = Quaternion.Euler(0f, 0f, -currentSafe.dialPosition * stepSize);
        }
    }

    void CheckInputConfirm()
    {
        CheckCurrentSafe();

        if (Input.GetKeyDown(KeyCode.Space) && currentSafe != null)
        {
            if (currentSafe.stageUnlocked)
            {
                currentSafe.confirmSound.Play(); // sonido de la barra solo si es correcto
                Debug.Log($"¡Número correcto en etapa {currentSafe.currentStage + 1}!");
                currentSafe.currentStage++;

                if (currentSafe.currentStage >= 3)
                    Debug.Log("¡Ganaste! Caja fuerte abierta.");
                else
                    currentSafe.stageUnlocked = false;
            }
            else
            {
                Debug.Log("No estás en el número correcto. Seguí buscando.");
            }
        }
    }

    void CheckCurrentSafe()
    {
        if (currentDial != null)
        {
            currentSafe = currentDial.GetComponent<SafeDial>();
        }
    }

    void MovePlayerToDials()
    {
        SafeDial[] totalSafes = totalOfDials;

        if (currentDial == null)
        {
            currentDial = totalSafes[countSafe].gameObject;
        }

        if (Input.GetKeyDown(KeyCode.A))
            currentDial = totalSafes[countSafe -= 1].gameObject;
        else if (Input.GetKeyDown(KeyCode.D))
            currentDial = totalSafes[countSafe += 1].gameObject;
    }

}
