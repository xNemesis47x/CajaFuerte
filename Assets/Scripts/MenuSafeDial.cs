using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSafeDial : MonoBehaviour
{
    public AudioSource correctSound; // sonido al acertar
    public AudioSource clickSound; // sonido de click
    public GameObject unlockImage; // Imagen que aparece cuando acierta
    public string gameSceneName = "Game"; // nombre de la escena a cargar

    public int dialPosition = 0; // Posición actual
    public int maxPositions = 100; // Total de posiciones (ej: 0-99)
    public float rotationStep = 3.6f; // 360 / 100 (para 100 posiciones)

    private int targetNumber; // Número random a alcanzar
    private bool isLocked = false; // Si ya llegó al número correcto

    void Start()
    {
        targetNumber = Random.Range(0, maxPositions);
        Debug.Log("Número correcto: " + targetNumber);

        if (unlockImage != null)
            unlockImage.SetActive(false);
    }

    void Update()
    {
        if (!isLocked)
        {
            // Solo permitir mover a la derecha con flechas
            if (Input.GetKeyDown(KeyCode.RightArrow)) // derecha
            {
                dialPosition = (dialPosition + 1) % maxPositions;
                transform.Rotate(Vector3.back * rotationStep); // gira como dial

                if (clickSound != null)
                    clickSound.Play();

                // Si llegamos al número correcto
                if (dialPosition == targetNumber)
                {
                    Debug.Log("¡Adivinaste el número!");
                    isLocked = true;

                    if (unlockImage != null)
                        unlockImage.SetActive(true);

                    if (correctSound != null)
                        correctSound.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) // izquierda
            {
                dialPosition = (dialPosition - 1 + maxPositions) % maxPositions;
                transform.Rotate(Vector3.forward * rotationStep); // gira hacia atrás

                if (clickSound != null)
                    clickSound.Play();

                // Si llegamos al número correcto
                if (dialPosition == targetNumber)
                {
                    Debug.Log("¡Adivinaste el número!");
                    isLocked = true;

                    if (unlockImage != null)
                        unlockImage.SetActive(true);

                    if (correctSound != null)
                        correctSound.Play();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(gameSceneName);
            }
        }
    }
}
