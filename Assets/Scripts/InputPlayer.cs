using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentDial;
    [SerializeField] private SafeDial[] totalOfDials;
    private SafeDial currentSafe;

    private float stepSize = 3.6f;        // 360° / 100 posiciones
    [SerializeField] private int countSafe = 0;

    private int unlockedSafes = 0;        // cuántos diales fueron abiertos

    [Header("Rotación")]
    public float rotationSpeed = 10f;     // pasos por segundo
    private float rotationTimer = 0f;     // acumulador de tiempo

    private void Start()
    {
        if (totalOfDials.Length > 0)
        {
            currentDial = totalOfDials[countSafe].gameObject;
            CheckCurrentSafe();
        }
    }

    private void Update()
    {
        MovePlayerToDials();
        HandleInput();
        CheckFinalConfirm();
    }

    void HandleInput()
    {
        if (currentSafe != null && !currentSafe.IsUnlocked)
        {
            int dir = 0;

            if (Input.GetKey(KeyCode.RightArrow)) dir = 1;
            else if (Input.GetKey(KeyCode.LeftArrow)) dir = -1;

            if (dir != 0)
            {
                rotationTimer += Time.deltaTime * rotationSpeed;

                while (rotationTimer >= 1f) // cada paso entero
                {
                    rotationTimer -= 1f;
                    currentSafe.Rotate(dir);
                    currentSafe.PlayStepSound();
                }
            }
            else
            {
                rotationTimer = 0f; // solté la tecla, reseteo acumulador
            }

            // Rotación visual
            currentSafe.transform.localRotation = Quaternion.Euler(0f, 0f, -currentSafe.dialPosition * stepSize);
        }
    }

    void CheckFinalConfirm()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentSafe != null)
        {
            if (currentSafe.IsCompleted())
            {
                Debug.Log($"¡Dial {currentSafe.dialName} desbloqueado!");

                if (!currentSafe.IsUnlocked)
                {
                    currentSafe.IsUnlocked = true;
                    unlockedSafes++;
                }

                if (unlockedSafes >= totalOfDials.Length)
                {
                    Debug.Log("¡Ganaste! Todos los diales desbloqueados, caja fuerte abierta.");
                }
            }
            else
            {
                Debug.Log("Todavía no completaste la combinación de este dial.");
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            countSafe = Mathf.Clamp(countSafe - 1, 0, totalOfDials.Length - 1);
            currentDial = totalOfDials[countSafe].gameObject;
            CheckCurrentSafe();
            Debug.Log($"Cambiado al dial {currentSafe.dialName}");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            countSafe = Mathf.Clamp(countSafe + 1, 0, totalOfDials.Length - 1);
            currentDial = totalOfDials[countSafe].gameObject;
            CheckCurrentSafe();
            Debug.Log($"Cambiado al dial {currentSafe.dialName}");
        }
    }
}
