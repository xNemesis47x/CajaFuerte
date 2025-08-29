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

    CountdownTimer timer;

    private void Start()
    {
        timer = FindObjectOfType<CountdownTimer>();

        CheckCurrentSafe();
        CheckPunteroPosition(currentSafe.transform);
    }

    private void Update()
    {
        HandleInput();
        QuitGame();
    }

    void HandleInput()
    {
        if (currentSafe != null && !currentSafe.IsUnlocked)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentSafe.Rotate(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentSafe.Rotate(-1);
            }
            currentSafe.transform.localRotation = Quaternion.Euler(0f, 0f, -currentSafe.dialPosition * stepSize);
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentSafe.isCompleted)
        {
            if (currentSafe.confirmSound != null) currentSafe.confirmSound.Play();
            timer.unlockedSafes++;
            currentSafe.IsUnlocked = true;
            currentSafe.confirm.SetActive(false);

            if (currentSafe.StageIndicator != null) currentSafe.StageIndicator.color = currentSafe.readyColor;

            timer.CheckWin();
            MovePlayerToDials();
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

    private void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
