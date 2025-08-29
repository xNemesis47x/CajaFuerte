using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SafeDial : MonoBehaviour
{
    public string dialName = "SinNombre"; // para debug: Izquierda / Medio / Derecha

    public int dialPosition = 0;
    public AudioSource clickSound;
    public AudioSource correctSound;
    public AudioSource confirmSound;

    public GameObject confirm;
    [SerializeField] private TMP_Text textCombination;
    public int[] combination = new int[3];
    private int currentStage = 0;

    private int lastMoveDir = 0;      // 1 = derecha, -1 = izquierda
    private int zeroPasses = 0;       // cuántas veces se pasó por cero en esta etapa
    private int lastPosition = 0;

    private int stageStartPos = 0;    // posición donde empieza cada etapa
    private int stepsAfterMinTurns = 0; // pasos contados después de las vueltas mínimas
    private int completeStages = 0;

    public bool IsUnlocked = false;
    public bool isCompleted;

    public static SafeDial currentDial; // el dial actualmente seleccionado

    [Header("Visual Indicator")]
    private SpriteRenderer stageIndicator; // asignar en inspector
    public Color normalColor = Color.white;
    public Color readyColor = Color.green;

    public SpriteRenderer StageIndicator { get => stageIndicator; set => stageIndicator = value; }

    private void Awake()
    {
        stageIndicator = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GenerateCombination();

        dialPosition = 0;
        stageStartPos = 0;
        stepsAfterMinTurns = 0;
        confirm.SetActive(false);

        if (stageIndicator != null)
            stageIndicator.color = normalColor;

        Debug.Log($"[Dial {dialName}] Combinación: {combination[0]} der(1 vuelta), {combination[1]} izq(1 vuelta), {combination[2]} der(1 vuelta)");
    }

    public void Rotate(int dir) // dir = 1 derecha, -1 izquierda
    {
        if (IsUnlocked) return;

        lastMoveDir = dir;
        lastPosition = dialPosition;

        // Cada tick de la ruedita = 1 paso (3.6 grados en tu modelo)
        dialPosition = (dialPosition + dir + 100) % 100;

        // Contar pasos por cero según la etapa y dirección
        if (currentStage == 0 && dir == 1 && lastPosition == 99 && dialPosition == 0) zeroPasses++;
        if (currentStage == 1 && dir == -1 && lastPosition == 0 && dialPosition == 99) zeroPasses++;
        if (currentStage == 2 && dir == 1 && lastPosition == 99 && dialPosition == 0) zeroPasses++;

        // Contar pasos después de las vueltas mínimas
        if ((currentStage == 0 && zeroPasses >= 1) ||
            (currentStage == 1 && zeroPasses >= 1) ||
            (currentStage == 2 && zeroPasses >= 1))
        {
            stepsAfterMinTurns++;
        }

        if (clickSound != null) clickSound.Play();

        Debug.Log($"[Dial {dialName}] Posición: {dialPosition}, Etapa: {currentStage}, ZeroPasses: {zeroPasses}, StepsAfterMinTurns: {stepsAfterMinTurns}");

        CheckStage();
    }

    private void CheckStage()
    {
        switch (currentStage)
        {
            case 0: // Etapa 0: derecha, mínimo 1 vuelta
                if (lastMoveDir != 1) { ResetSafe("Debías girar a la derecha."); return; }

                if (zeroPasses >= 1)
                {
                    int targetSteps = ((100 - combination[0]) % 100) + 1;
                    if (stepsAfterMinTurns == targetSteps)
                    {
                        if (correctSound != null) correctSound.Play();
                        Debug.Log($"[Dial {dialName}] Primer número correcto ({combination[0]}).");
                        currentStage++;
                        zeroPasses = 0;
                        stepsAfterMinTurns = 0;
                        stageStartPos = dialPosition;
                        if (stageIndicator != null) stageIndicator.color = normalColor;

                        if (completeStages == 0)
                        {
                            textCombination.text = $"{combination[0]}";
                            completeStages++;
                        }
                    }
                }
                break;

            case 1: // Etapa 1: izquierda, mínimo 1 vuelta
                if (lastMoveDir != -1) { ResetSafe("Debías girar a la izquierda."); return; }

                if (zeroPasses >= 1)
                {
                    int targetSteps = combination[1];
                    if (stepsAfterMinTurns == targetSteps)
                    {
                        if (correctSound != null) correctSound.Play();
                        Debug.Log($"[Dial {dialName}] Segundo número correcto ({combination[1]}).");
                        currentStage++;
                        zeroPasses = 0;
                        stepsAfterMinTurns = 0;
                        stageStartPos = dialPosition;
                        if (stageIndicator != null) stageIndicator.color = normalColor;

                        if (completeStages == 1)
                        {
                            textCombination.text += $", {combination[1]}";
                            completeStages++;
                        }
                    }
                }
                break;

            case 2: // Etapa 2: derecha directa con mínimo 1 vuelta
                if (lastMoveDir != 1) { ResetSafe("Debías girar a la derecha."); return; }

                int targetSteps2 = ((100 - combination[2]) % 100) + 1;

                if (zeroPasses >= 1 && stepsAfterMinTurns == targetSteps2)
                {
                    if (correctSound != null) correctSound.Play();
                    Debug.Log($"[Dial {dialName}] ¡Combinación completa ({combination[2]})!");
                    currentStage++;
                    isCompleted = true;

                    confirm.SetActive(true);

                    if (completeStages == 2)
                    {
                        completeStages++;
                        textCombination.text += $", {combination[2]}";
                    }
                }
                break;
            case 3:
                if (lastMoveDir != -1 || lastMoveDir != 1) { ResetSafe("Debías girar a la derecha."); return; }
                break;
        }
    }

    private void ResetSafe(string motivo)
    {
        Debug.Log($"[Dial {dialName}] {motivo} → Reiniciando combinación.");
        currentStage = 0;
        zeroPasses = 0;
        stepsAfterMinTurns = 0;
        dialPosition = 0;
        stageStartPos = 0;
        isCompleted = false;

        confirm.SetActive(false);

        if (stageIndicator != null)
            stageIndicator.color = normalColor;
    }

    public bool IsCompleted()
    {
        return IsUnlocked;
    }

    void GenerateCombination()
    {
        for (int i = 0; i < 3; i++)
            combination[i] = Random.Range(1, 90);
    }
}
