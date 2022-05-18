using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationObject 
{
    public FighterDataObject firstCharacter;
    public FighterDataObject secondCharacter;

    public List<string> firstCharacterDialogue;
    public List<string> secondCharacterDialogue;

    public bool firstCharacterStartsDialogue = false;
    public bool firstCharacterIsPlayer = true;

    public int dialogueChainIndex = 0;

    public ConversationObject()
    {
        firstCharacterDialogue = new List<string>();
        secondCharacterDialogue = new List<string>();
    }
}
