using UnityEngine;
using UnityEngine.UI;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private GameObject currentDial;
    [SerializeField] private SafeDial[] totalOfDials;
    private SafeDial currentSafe;

    private float stepSize = 3.6f;        // 360° / 100 posiciones
    [SerializeField] private int countSafe = 0;
    public GameObject puntero;

    private float stepDelay = 0.1f; // tiempo entre pasos
    private float stepTimer = 0f;

    private void Start()
    {
        CheckCurrentSafe();
        CheckPunteroPosition(currentSafe.transform);
    }

    private void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (currentSafe != null && !currentSafe.IsUnlocked)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    currentSafe.Rotate(1);
                    stepTimer = stepDelay;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    currentSafe.Rotate(-1);
                    stepTimer = stepDelay;
                }
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

    public void MovePlayerToDials()
    {
        countSafe = Mathf.Clamp(countSafe + 1, 0, totalOfDials.Length - 1);
        currentDial = totalOfDials[countSafe].gameObject;
        CheckCurrentSafe();
        CheckPunteroPosition(currentSafe.transform);
        Debug.Log($"Cambiado al dial {currentSafe.dialName}");
    }

    void CheckPunteroPosition(Transform transform)
    {
        Vector3 posPunter = puntero.transform.position;
        posPunter.x = transform.position.x;
        puntero.transform.position = posPunter;
    }
}
