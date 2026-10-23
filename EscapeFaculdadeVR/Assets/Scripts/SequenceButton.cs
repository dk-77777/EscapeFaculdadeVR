using UnityEngine;

public class SequenceButton : MonoBehaviour
{
    [SerializeField] private SequencePuzzle puzzle;
    [SerializeField] private string id = "JAVA";

    public void Press()
    {
        if (puzzle != null)
            puzzle.Press(id);
    }
}
