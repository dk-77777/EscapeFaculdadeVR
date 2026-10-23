using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencePuzzle : MonoBehaviour
{
    [Tooltip("Exemplo: JAVA, REDES, BD, IA")]
    [SerializeField] private List<string> correctSequence = new List<string>();

    public UnityEvent onSolved;
    public UnityEvent onMistake;

    private int currentIndex;
    private bool solved;

    public void Press(string id)
    {
        if (solved || correctSequence.Count == 0)
            return;

        if (id == correctSequence[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= correctSequence.Count)
            {
                solved = true;
                onSolved?.Invoke();
            }
        }
        else
        {
            currentIndex = 0;
            onMistake?.Invoke();
        }
    }

    public void ResetPuzzle()
    {
        currentIndex = 0;
        solved = false;
    }
}
