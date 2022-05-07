using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Dialogue", menuName = "AI/Dialogue Module")]

public class SOAIDialogueObject : ScriptableObject
{
    [TextArea(5, 10)]
    [SerializeField] private string introDialogue;
    [TextArea(5, 10)]
    [SerializeField] private string aIWinDialogue;
    [TextArea(5, 10)]
    [SerializeField] private string aILoseDialogue;
    [TextArea(5, 10)]
    [SerializeField] private List<string> randomFightDialogue;

    public string IntroDialogue { get => introDialogue; }
    public string AIWinDialogue { get => aIWinDialogue; }
    public string AILoseDialogue { get => aILoseDialogue; }
    public List<string> RandomFightDialogue { get => randomFightDialogue; }
}
