using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOMechComponent testMechHead;
    [SerializeField] private SOMechComponent testMechTorso;
    [SerializeField] private SOMechComponent testMechArms;
    [SerializeField] private SOMechComponent testMechLegs;
    [SerializeField] private int testMaxHP;
    [SerializeField] private int testMaxEnergy;
    [SerializeField] private List<SOCardDataObject> testDeck;

    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        MechHeadObject head = new MechHeadObject(testMechHead);
        MechTorsoObject torso = new MechTorsoObject(testMechTorso);
        MechLegsObject legs = new MechLegsObject(testMechLegs);
        MechArmsObject arms = new MechArmsObject(testMechArms);

        MechObject playerMech = new MechObject(head, torso, arms, legs, testMaxHP, testMaxEnergy);
        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerMech, playerPilot, testDeck);

        head = new MechHeadObject(testMechHead);
        torso = new MechTorsoObject(testMechTorso);
        legs = new MechLegsObject(testMechLegs);
        arms = new MechArmsObject(testMechArms);

        MechObject opponentMech = new MechObject(head, torso, arms, legs, testMaxHP, testMaxEnergy);
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
