using UnityEngine;

public class SafeDial : MonoBehaviour
{
    public string dialName = "SinNombre"; // para debug: Izquierda / Medio / Derecha

    public int dialPosition = 0;
    public AudioSource clickSound;
    public AudioSource correctSound;
    public AudioSource confirmSound;

    private int[] combination = new int[3];
    private int currentStage = 0;

    private int lastMoveDir = 0;      // 1 = derecha, -1 = izquierda
    private int zeroPasses = 0;       // cuántas veces se pasó por cero en esta etapa
    private int lastPosition = 0;

    private int stageStartPos = 0;    // posición donde empieza cada etapa
    private int stepsAfterMinTurns = 0; // pasos contados después de las vueltas mínimas

    public bool IsUnlocked = false;

    public static SafeDial currentDial; // el dial actualmente seleccionado

    private void Start()
    {
        GenerateCombination();

        dialPosition = 0;
        stageStartPos = 0;
        stepsAfterMinTurns = 0;

        Debug.Log($"[Dial {dialName}] Combinación: {combination[0]} der(2 vueltas), {combination[1]} izq(2 pasos por cero), {combination[2]} der(directa)");
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
        if (currentStage == 2 && dir == 1 && lastPosition == 99 && dialPosition == 0) zeroPasses++; // cuenta cada paso por cero

        // Contar pasos después de las vueltas mínimas
        if ((currentStage == 0 && zeroPasses >= 2 && dir == 1) ||
            (currentStage == 1 && zeroPasses >= 2 && dir == -1) ||
            (currentStage == 2 && zeroPasses >= 2 && dir == 1)) // <-- 2 vueltas obligatorias ahora
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
            case 0: // Etapa 0: derecha, mínimo 2 vueltas
                if (lastMoveDir != 1) { ResetSafe("Debías girar a la derecha."); return; }

                if (zeroPasses >= 2)
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
                    }
                }
                break;

            case 1: // Etapa 1: izquierda, mínimo 2 pasos por cero
                if (lastMoveDir != -1) { ResetSafe("Debías girar a la izquierda."); return; }

                if (zeroPasses >= 2)
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
                    }
                }
                break;

            case 2: // Etapa 2: derecha directa con vuelta extra
                if (lastMoveDir != 1) { ResetSafe("Debías girar a la derecha."); return; }

                int targetSteps2 = ((100 - combination[2]) % 100) + 1;

                // Validar solo si ya se hizo la vuelta obligatoria
                if (zeroPasses >= 1 && stepsAfterMinTurns == targetSteps2)
                {
                    if (correctSound != null) correctSound.Play();
                    if (confirmSound != null) confirmSound.Play();
                    Debug.Log($"[Dial {dialName}] ¡Combinación completa ({combination[2]})!");
                    currentStage++;
                    IsUnlocked = true;
                }
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
    }

    public bool IsCompleted()
    {
        return IsUnlocked;
    }

    void GenerateCombination()
    {
        for (int i = 0; i < 3; i++)
            combination[i] = Random.Range(0, 90);
    }
}
