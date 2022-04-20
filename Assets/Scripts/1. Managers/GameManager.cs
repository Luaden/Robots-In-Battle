using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SOItemDataObject testMechHead;
    [SerializeField] private SOItemDataObject testMechTorso;
    [SerializeField] private SOItemDataObject testMechArms;
    [SerializeField] private SOItemDataObject testMechLegs;
    [SerializeField] private List<SOItemDataObject> testDeck;

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
        CombatManager.instance.MechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
        CombatManager.instance.MechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy);
        CombatManager.instance.OpponentFighter = opponentFighter;
        CombatManager.instance.MechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
        CombatManager.instance.MechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy);

        DrawCardPrefab();
    }

    private void DrawCardPrefab()
    {
        CombatManager.instance.DeckManager.SetPlayerDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawPlayerCard();

        CombatManager.instance.DeckManager.SetOpponentDeck(testDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawOpponentCard();

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
