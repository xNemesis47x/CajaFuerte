using UnityEngine;

public class SafeDial : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource correctSound;
    [field: SerializeField] public AudioSource confirmSound;
    public int dialPosition = 0;       // 0..89
    private int[] combination = new int[3];
    public int currentStage = 0;       // 0–2
    public bool stageUnlocked = false;

    void Start()
    {
        GenerateCombination();
        Debug.Log($"Combinación: {combination[0]} - {combination[1]} - {combination[2]}");
    }

    public void PlayStepSound()
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

    void GenerateCombination()
    {
        for (int i = 0; i < 3; i++)
            combination[i] = Random.Range(0, 90);
    }
}
