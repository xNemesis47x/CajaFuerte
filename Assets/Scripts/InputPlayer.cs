using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentDial;
    [SerializeField] private SafeDial[] totalOfDials;
    private SafeDial currentSafe;

    private float stepSize = 3.6f;        // 360° / 100 posiciones
    [SerializeField] private int countSafe = 0;
    public GameObject puntero;

    private int unlockedSafes = 0;        // cuántos diales fueron abiertos

    private void Start()
    {
        CheckCurrentSafe();
        CheckPunteroPosition(currentSafe.transform);
    }

    private void Update()
    {
        MovePlayerToDials();
        HandleInput();
    }

    void HandleInput()
    {
        if (currentSafe != null && !currentSafe.IsUnlocked)
        {
            float scroll = -Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                currentSafe.Rotate(1); // derecha
            }
            else if (scroll < 0f)
            {
                currentSafe.Rotate(-1); // izquierda
            }

            currentSafe.transform.localRotation = Quaternion.Euler(0f, 0f, -currentSafe.dialPosition * stepSize);
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
        Vector3 posPunter = puntero.transform.position;

        if (Input.GetKeyDown(KeyCode.A))
        {
            countSafe = Mathf.Clamp(countSafe - 1, 0, totalOfDials.Length - 1);
            currentDial = totalOfDials[countSafe].gameObject;
            CheckCurrentSafe();
            CheckPunteroPosition(currentSafe.transform);
            Debug.Log($"Cambiado al dial {currentSafe.dialName}");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            countSafe = Mathf.Clamp(countSafe + 1, 0, totalOfDials.Length - 1);
            currentDial = totalOfDials[countSafe].gameObject;
            CheckCurrentSafe();
            CheckPunteroPosition(currentSafe.transform);
            Debug.Log($"Cambiado al dial {currentSafe.dialName}");
        }
    }

    void CheckPunteroPosition(Transform transform)
    {
        Vector3 posPunter = puntero.transform.position;
        posPunter.x = transform.position.x;
        puntero.transform.position = posPunter;
    }
}
