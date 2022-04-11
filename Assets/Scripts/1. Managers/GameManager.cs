using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOMechComponent testMechHead;
    [SerializeField] private SOMechComponent testMechTorso;
    [SerializeField] private SOMechComponent testMechArms;
    [SerializeField] private SOMechComponent testMechLegs;
    [SerializeField] private List<SOCardDataObject> testDeck;

    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        MechComponentDataObject head = new MechComponentDataObject(testMechHead);
        MechComponentDataObject torso = new MechComponentDataObject(testMechTorso);
        MechComponentDataObject legs = new MechComponentDataObject(testMechLegs);
        MechComponentDataObject arms = new MechComponentDataObject(testMechArms);

        MechObject playerMech = new MechObject(head, torso, arms, legs);
        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerMech, playerPilot, testDeck);

        head = new MechComponentDataObject(testMechHead);
        torso = new MechComponentDataObject(testMechTorso);
        legs = new MechComponentDataObject(testMechLegs);
        arms = new MechComponentDataObject(testMechArms);

        MechObject opponentMech = new MechObject(head, torso, arms, legs);
        PilotDataObject opponentPilot = new PilotDataObject();
        FighterDataObject opponentFighter = new FighterDataObject(opponentMech, opponentPilot, testDeck);

        CombatManager.instance.PlayerFighter = playerFighter;
        CombatManager.instance.OpponentFighter = opponentFighter;

        DrawCardPrefab();
    }

    public void DrawCardPrefab()
    {
        CombatManager.instance.DeckManager.SetPlayerDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawPlayerCard();

        CombatManager.instance.DeckManager.SetOpponentDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawOpponentCard();
    }
}
