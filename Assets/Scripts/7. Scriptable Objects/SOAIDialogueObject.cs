using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Dialogue", menuName = "AI/Dialogue Module")]

public class SOAIDialogueObject : ScriptableObject
{
    [TextArea(5, 10)]
    [SerializeField] private List<string> introDialogue;
    [TextArea(5, 10)]
    [SerializeField] private List<string> introResponseDialogue;
    [TextArea(5, 10)]
    [SerializeField] private List<string> aIWinDialogue;
    [TextArea(5, 10)]
    [SerializeField] private List<string> aILoseDialogue;
    [TextArea(5, 10)]
    [SerializeField] private List<string> randomFightDialogue;

    public List<string> IntroDialogue { get => introDialogue; }
    public List<string> AIWinDialogue { get => aIWinDialogue; }
    public List<string> AILoseDialogue { get => aILoseDialogue; }
    public List<string> RandomFightDialogue { get => randomFightDialogue; }
}
