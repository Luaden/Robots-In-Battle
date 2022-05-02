using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentOverviewManager : MonoBehaviour
{
    private NodeSlotManager nodeSlotManager;
    /* 
     * 
     * reference to playerfights
    Node tree 
    * List<nodes> containing info about fighters, placements
        * each node contains parent/previous node
            * fighter pair objects
     UI controllers to be able to move player & enemy 
    
    fight button for completing task

    stats displayed (hp, money, time, ??)

    pilots that can be dragged to first set of nodes
     
     */

    private void Awake()
    {
        nodeSlotManager = FindObjectOfType<NodeSlotManager>();
    }
    private void Start()
    {
    }
}
