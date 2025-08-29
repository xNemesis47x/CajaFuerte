using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- necesario para cambiar escenas

public class MenuSafeDial : MonoBehaviour
{
    public int dialPosition = 0; // Posición actual
    public int maxPositions = 100; // Total de posiciones (ej: 0-99)
    public float rotationStep = 3.6f; // 360 / 100 (para 100 posiciones)

    private int targetNumber; // Número random a alcanzar
    private bool isLocked = false; // Si ya llegó al número correcto

    public AudioSource clickSound;
    public GameObject unlockImage; // Imagen que aparece cuando acierta

    public string gameSceneName = "Game"; // nombre de la escena a cargar

    void Start()
    {
        // Elegimos un número random entre 0 y maxPositions-1
        targetNumber = Random.Range(0, maxPositions);
        Debug.Log("Número correcto: " + targetNumber);

        if (unlockImage != null)
            unlockImage.SetActive(false);
    }

    void Update()
    {
        // Si ya está bloqueado, no permitir mover
        if (!isLocked)
        {
            // Solo permitir mover a la derecha
            if (Input.GetKeyDown(KeyCode.D)) // tecla D para girar derecha
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
                }
            }
        }
        else
        {
            // Una vez acertado, presionando barra se cambia de escena
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(gameSceneName);
            }
        }
    }
}
