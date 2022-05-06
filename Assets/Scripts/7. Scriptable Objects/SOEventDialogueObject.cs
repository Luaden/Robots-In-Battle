using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Dialogue", menuName = "Events/Event Dialogue Module")]

public class SOEventDialogueObject : ScriptableObject
{
    [TextArea(5, 10)]
    [SerializeField] private List<string> introText;
    [TextArea(5, 10)]
    [SerializeField] private string affirmationText;
    [TextArea(5, 10)]
    [SerializeField] private string rejectionText;
    //Debug.Log("WAT")
}
