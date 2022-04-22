using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Playtesting
    [SerializeField] private SOItemDataObject starterMechHead;
    [SerializeField] private SOItemDataObject starterMechTorso;
    [SerializeField] private SOItemDataObject starterMechArms;
    [SerializeField] private SOItemDataObject starterMechLegs;
    [SerializeField] private List<SOItemDataObject> starterDeck;

    [ContextMenu("Start Game")]
    public void BuildMech()
    {
        MechComponentDataObject head = new MechComponentDataObject(starterMechHead);
        MechComponentDataObject torso = new MechComponentDataObject(starterMechTorso);
        MechComponentDataObject legs = new MechComponentDataObject(starterMechLegs);
        MechComponentDataObject arms = new MechComponentDataObject(starterMechArms);

        MechObject playerMech = new MechObject(head, torso, arms, legs);
        PilotDataObject playerPilot = new PilotDataObject();
        FighterDataObject playerFighter = new FighterDataObject(playerMech, playerPilot, starterDeck);

        head = new MechComponentDataObject(starterMechHead);
        torso = new MechComponentDataObject(starterMechTorso);
        legs = new MechComponentDataObject(starterMechLegs);
        arms = new MechComponentDataObject(starterMechArms);

        MechObject opponentMech = new MechObject(head, torso, arms, legs);
        PilotDataObject opponentPilot = new PilotDataObject();
        FighterDataObject opponentFighter = new FighterDataObject(opponentMech, opponentPilot, starterDeck);

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
        CombatManager.instance.DeckManager.SetPlayerDeck(starterDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawPlayerCard();

        CombatManager.instance.DeckManager.SetOpponentDeck(starterDeck);

        for (int i = 0; i <= 4; i++)
            CombatManager.instance.DeckManager.DrawOpponentCard();

    }
    #endregion
    private PlayerDataObject playerData;

    public static GameManager instance;

    public PlayerDataObject PlayerData { get => playerData; set => playerData = value; }
    

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        
    }

    public void LoadPlayer(PlayerDataObject playerDataObject = null)
    {
        if (playerDataObject != null)
        {
            playerData = playerDataObject;
            return;
        }

        //Need default deck and default mech stored here.
        playerData = new PlayerDataObject();
    }
}
