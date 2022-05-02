using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    private FighterPairObject currentFighterPairObject;
    private NodeController previousNode;
    private NodeController nextNode;

    public void AssignFighterPairObjectToNode(FighterPairObject fighterPair)
    {
        currentFighterPairObject = fighterPair;
    }




}
