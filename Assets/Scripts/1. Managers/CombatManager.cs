using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private int winsBeforeBoss;
    [Space]
    [Header("Combat Modifiers")]
    [SerializeField] private int mechEnergyGain;
    [SerializeField] private float brokenComponentDamageMultiplier;
    [SerializeField] private float counterDamageMultiplier;
    [SerializeField] private float guardDamageMultiplier;
    [SerializeField] private float acidComponentDamageMultiplier;
    [SerializeField] private float iceChannelEnergyReductionModifier;
    [Space]
    [Header("Scene References")]
    [SerializeField] private GameObject winLossPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject lossPanel;
    [SerializeField] private GameObject fightButton;
    [Space]

    #region Debug
    [Header("Debug / Testing")]
    [SerializeField] private bool narrateCardSelection;
    [SerializeField] private bool narrateCombat;
    [SerializeField] private bool narrateEffects;
    [SerializeField] private bool narrateAIDecisionMaking;
    [SerializeField] private bool displayAIDecisionIndicator;
    [SerializeField] private bool playPilotEffectsOnFirstTurn;

    public bool NarrateCardSelection { get => narrateCardSelection; }
    public bool NarrateCombat { get => narrateCombat; }
    public bool NarrateEffects { get => narrateEffects; }
    public bool NarrateAIDecisionMaking { get => narrateAIDecisionMaking; }
    public bool DisplayAIDecisionIndicator { get => displayAIDecisionIndicator; }
    #endregion

    public static CombatManager instance;

    private CombatDeckManager deckManager;
    private HandManager handManager;
    private PlayerHandUISlotManager playerHandSlotManager;
    private OpponentHandUISlotManager opponentHandSlotManager;
    private ChannelsUISlotManager channelsUISlotManager;
    private CardPlayManager cardPlayManager;
    private CardUIManager cardUIManager;
    private MechHUDManager mechHUDManager;
    private PopupUIManager popupUIManager;
    private BuffUIManager buffUIManager;
    private CombatAnimationManager combatAnimationManager;
    private CombatEffectManager combatEffectManager;
    private PilotEffectManager pilotEffectManager;
    private CombatSequenceManager combatSequenceManager;
    private AIManager aIManager;
    private MechSpriteSwapManager mechSpriteSwapManager;
    private StatTrackerController statTrackerController;
    private PlayerInventoryCardDeckUISlotManager playerInventoryCardDeckSlotManager;
    private OpponentInventoryCardDeckUISlotManager opponentInventoryCardDeckSlotManager;
    private CardClickController cardClickController;

    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;

    private bool canPlayCards = true;
    private bool hasStartedGame = false;
    private bool hasNotCompletedIntroDialogue = true;
    private bool hasStartedCombat = false;
    private bool gameOver = false;
    private bool hasWon = false;
    private bool hasLost = false;

    public FighterDataObject PlayerFighter { get => playerFighter; set => InitPlayerFighter(value); }
    public FighterDataObject OpponentFighter { get => opponentFighter; set => InitOpponentFighter(value); }
    public CombatDeckManager DeckManager { get => deckManager; }
    public HandManager HandManager { get => handManager; }
    public PlayerHandUISlotManager PlayerHandSlotManager { get => playerHandSlotManager; }
    public OpponentHandUISlotManager OpponentHandSlotManager { get => opponentHandSlotManager; }
    public ChannelsUISlotManager ChannelsUISlotManager { get => channelsUISlotManager; }
    public CardPlayManager CardPlayManager { get => cardPlayManager; }
    public CardUIManager CardUIManager { get => cardUIManager; }
    public PopupUIManager PopupUIManager { get => popupUIManager; }
    public BuffUIManager BuffUIManager { get => buffUIManager; }
    public CombatAnimationManager CombatAnimationManager { get => combatAnimationManager; }
    public CombatEffectManager CombatEffectManager { get => combatEffectManager; }
    public PilotEffectManager PilotEffectManager { get => pilotEffectManager; }
    public CombatSequenceManager CombatSequenceManager { get => combatSequenceManager; }
    public AIManager AIManager { get => aIManager; }
    public MechSpriteSwapManager MechSpriteSwapManager { get => mechSpriteSwapManager; }
    public StatTrackerController StatTrackerController { get => statTrackerController; }
    public PlayerInventoryCardDeckUISlotManager PlayerInventoryCardDeckSlotManager { get => playerInventoryCardDeckSlotManager; }
    public OpponentInventoryCardDeckUISlotManager OpponentInventoryCardDeckSlotManager { get => opponentInventoryCardDeckSlotManager; }
    public CardClickController CardClickController { get => cardClickController; }

    public int MechEnergyGain { get => mechEnergyGain; }
    public float BrokenCDM { get => brokenComponentDamageMultiplier; }
    public float CounterDamageMultiplier { get => counterDamageMultiplier; }
    public float GuardDamageMultiplier { get => guardDamageMultiplier; }
    public float AcidComponentDamageMultiplier { get => acidComponentDamageMultiplier; }
    public float IceChannelEnergyReductionModifier { get => iceChannelEnergyReductionModifier; }
    public bool CanPlayCards { get => canPlayCards; }
    public bool GameOver { get => gameOver; }


    public delegate void onDestroyScene();
    public static event onDestroyScene OnDestroyScene;

    public delegate void onStartNewTurn();
    public static event onStartNewTurn OnStartNewTurn;

    public void RemoveHealthFromMech(DamageMechPairObject damageMechPair)
    {
        if (NarrateCombat)
        {
            Debug.Log(CharacterSelect.Player + " starting HP: " + instance.PlayerFighter.FighterMech.MechCurrentHP.ToString());
            Debug.Log(CharacterSelect.Opponent + " starting HP: " + instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
        }

        if (damageMechPair.CharacterTakingDamage == CharacterSelect.Player)
        {
            foreach(Channels channel in GetChannelListFromFlags(damageMechPair.GetDamageChannels()))
            {
                switch (channel)
                {
                    case Channels.High:
                        playerFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel, 
                                damageMechPair.CharacterTakingDamage), MechComponent.Head);

                        if (playerFighter.FighterMech.MechHead.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Head, CharacterSelect.Player);
                        break;

                    case Channels.Mid:
                        playerFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel,
                                damageMechPair.CharacterTakingDamage), MechComponent.Torso);

                        if (playerFighter.FighterMech.MechTorso.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Torso, CharacterSelect.Player);
                        break;

                    case Channels.Low:
                        playerFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel,
                                damageMechPair.CharacterTakingDamage), MechComponent.Legs);

                        if (playerFighter.FighterMech.MechLegs.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Legs, CharacterSelect.Player);
                        break;                
                }
            }
        }

        if (damageMechPair.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            foreach (Channels channel in GetChannelListFromFlags(damageMechPair.GetDamageChannels()))
            {
                switch (channel)
                {
                    case Channels.High:
                        opponentFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel,
                                damageMechPair.CharacterTakingDamage), MechComponent.Head);

                        if (opponentFighter.FighterMech.MechHead.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Head, CharacterSelect.Opponent);
                        break;

                    case Channels.Mid:
                        opponentFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel,
                                damageMechPair.CharacterTakingDamage), MechComponent.Torso);

                        if (opponentFighter.FighterMech.MechTorso.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Torso, CharacterSelect.Opponent);
                        break;

                    case Channels.Low:
                        opponentFighter.FighterMech.DamageComponentHP(
                            combatEffectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageWithAndConsumeModifiers(), channel,
                                damageMechPair.CharacterTakingDamage), MechComponent.Legs);

                        if (opponentFighter.FighterMech.MechLegs.ComponentCurrentHP <= 0)
                            combatAnimationManager.BreakComponent(MechComponent.Legs, CharacterSelect.Opponent);
                        break;
                }
            }
        }

        if (NarrateCombat)
        {
            Debug.Log(CharacterSelect.Player + " ending HP: " + instance.PlayerFighter.FighterMech.MechCurrentHP.ToString());
            Debug.Log(CharacterSelect.Opponent + " ending HP: " + instance.OpponentFighter.FighterMech.MechCurrentHP.ToString());
        }

        mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
        mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
    }

    public void RemoveHealthFromMech(DamageEffectMechPairObject effectDamage)
    {
        if (effectDamage.characterToTakeDamage == CharacterSelect.Player)
            playerFighter.FighterMech.DamageComponentHP(effectDamage.damageToTake, MechComponent.None);
        else
            opponentFighter.FighterMech.DamageComponentHP(effectDamage.damageToTake, MechComponent.None);

        mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
        mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
    }

    public void AddHealthToMech(CharacterSelect character, int health)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentHP += health;
            mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }

        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentHP += health;
            mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);
            CheckForWinLoss();
        }
    }

    public void RemoveEnergyFromMechs(EnergyRemovalObject newEnergyRemovalObject)
    {
        if (newEnergyRemovalObject == null)
            return;

        if (newEnergyRemovalObject.firstMech == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentEnergy -= newEnergyRemovalObject.firstMechEnergyRemoval;

            if (newEnergyRemovalObject.secondMechEnergyRemoval != 0)
                opponentFighter.FighterMech.MechCurrentEnergy -= newEnergyRemovalObject.secondMechEnergyRemoval; 
        }
        if (newEnergyRemovalObject.firstMech == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentEnergy -= newEnergyRemovalObject.firstMechEnergyRemoval;

            if (newEnergyRemovalObject.secondMechEnergyRemoval != 0)
                playerFighter.FighterMech.MechCurrentEnergy -= newEnergyRemovalObject.secondMechEnergyRemoval;
        }

        mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
        mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, opponentFighter.FighterMech.MechCurrentEnergy, false);
    }

    public void PreviewEnergyConsumption(CharacterSelect character, int energyToRemove)
    {
        if (character == CharacterSelect.Player)
        {
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy - energyToRemove, true);
            return;
        }
        if (character == CharacterSelect.Opponent)
        {
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, opponentFighter.FighterMech.MechCurrentEnergy - energyToRemove, true);
            return;
        }
    }

    public void AddEnergyToMech(CharacterSelect character, int energyToAdd)
    {
        if (character == CharacterSelect.Player)
        {
            playerFighter.FighterMech.MechCurrentEnergy = Mathf.Clamp(playerFighter.FighterMech.MechCurrentEnergy + energyToAdd, 0, playerFighter.FighterMech.MechMaxEnergy);
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
        }
        if (character == CharacterSelect.Opponent)
        {
            opponentFighter.FighterMech.MechCurrentEnergy = Mathf.Clamp(opponentFighter.FighterMech.MechCurrentEnergy + energyToAdd, 0, opponentFighter.FighterMech.MechMaxEnergy);
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, opponentFighter.FighterMech.MechCurrentEnergy, false);
        }
    }

    public void ResetMechEnergyHUD(CharacterSelect character)
    {
        if (character == CharacterSelect.Player)
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, 0, false);
        if(character == CharacterSelect.Opponent)
            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
    }

    public List<Channels> GetChannelListFromFlags(Channels channelToInterpret)
    {
        List<Channels> channelList = new List<Channels>();

        if (channelToInterpret.HasFlag(Channels.High))
            channelList.Add(Channels.High);
        if (channelToInterpret.HasFlag(Channels.Mid))
            channelList.Add(Channels.Mid);
        if (channelToInterpret.HasFlag(Channels.Low))
            channelList.Add(Channels.Low);

        return channelList;
    }

    public void LoadWorkshop()
    {
        GameManager.instance.LoadWorkshopScene();
    }

    public void LoadTitleScene()
    {
        GameManager.instance.LoadTitleScene();
    }

    private void Awake()
    {
        instance = this;

        deckManager = FindObjectOfType<CombatDeckManager>(true);
        handManager = FindObjectOfType<HandManager>(true);
        playerHandSlotManager = FindObjectOfType<PlayerHandUISlotManager>(true);
        opponentHandSlotManager = FindObjectOfType<OpponentHandUISlotManager>(true);
        channelsUISlotManager = FindObjectOfType<ChannelsUISlotManager>(true);
        cardPlayManager = FindObjectOfType<CardPlayManager>(true);
        cardUIManager = FindObjectOfType<CardUIManager>(true);
        mechHUDManager = FindObjectOfType<MechHUDManager>(true);
        popupUIManager = FindObjectOfType<PopupUIManager>(true);
        buffUIManager = FindObjectOfType<BuffUIManager>(true);
        combatAnimationManager = FindObjectOfType<CombatAnimationManager>(true);
        combatEffectManager = FindObjectOfType<CombatEffectManager>(true);
        pilotEffectManager = FindObjectOfType<PilotEffectManager>(true);
        combatSequenceManager = FindObjectOfType<CombatSequenceManager>(true);
        aIManager = FindObjectOfType<AIManager>(true);
        mechSpriteSwapManager = FindObjectOfType<MechSpriteSwapManager>(true);
        statTrackerController = FindObjectOfType<StatTrackerController>(true);
        playerInventoryCardDeckSlotManager = FindObjectOfType<PlayerInventoryCardDeckUISlotManager>(true);
        cardClickController = FindObjectOfType<CardClickController>(true);
        opponentInventoryCardDeckSlotManager = FindObjectOfType<OpponentInventoryCardDeckUISlotManager>(true);
    }

    private void Start()
    {
        CombatSequenceManager.OnCombatStart += DisableCanPlayCards;
        CombatSequenceManager.OnRoundComplete += CheckForWinLoss;
        CombatSequenceManager.OnCombatComplete += CheckForWinLoss;

        AIDialogueController.OnDialogueComplete += StartNewTurnCheck;
        AIDialogueController.OnDialogueComplete += EnableCanPlayCards;

        if (GameManager.instance.Player.PlayerFighterData == null)
            Debug.Log("No player.");

        if (GameManager.instance.Player.OtherFighters[0] == null)
            Debug.Log("No Opponent.");

        InitPlayerFighter(GameManager.instance.Player.PlayerFighterData);

        if (GameManager.instance.PlayerWins == 0)
            InitOpponentFighter(GameManager.instance.Player.BossFighters[0]);
        else if (GameManager.instance.PlayerWins == winsBeforeBoss)
        {
            InitOpponentFighter(GameManager.instance.Player.BossFighters[1]);
            combatAnimationManager.SetMechStartingAnimations(opponentFighter.FighterMech, CharacterSelect.Opponent, true);
        }
        else
            InitOpponentFighter(GameManager.instance.Player.OtherFighters[GameManager.instance.PlayerWins]);

        pilotEffectManager.InitPilotEffectManager();
        statTrackerController.InitializeStatTracking();

        StartNewTurn();
    }

    private void OnDestroy()
    {
        OnDestroyScene?.Invoke();
        instance = null;

        CombatSequenceManager.OnCombatStart -= DisableCanPlayCards;
        CombatSequenceManager.OnRoundComplete -= CheckForWinLoss;
        CombatSequenceManager.OnCombatComplete -= CheckForWinLoss;

        AIDialogueController.OnDialogueComplete -= StartNewTurnCheck;
        AIDialogueController.OnDialogueComplete -= EnableCanPlayCards;
    }

    private void DisableCanPlayCards()
    {
        canPlayCards = false;
        hasStartedCombat = true;
    }

    private void EnableCanPlayCards()
    {
        canPlayCards = true;
    }

    private void InitPlayerFighter(FighterDataObject newPlayerFighter)
    {
        playerFighter = newPlayerFighter;
        playerFighter.FighterMech.ResetEnergy();

        deckManager.SetPlayerDeck(newPlayerFighter.FighterDeck);

        mechHUDManager.SetPlayerMaxStats(playerFighter.FighterMech.MechMaxHP, playerFighter.FighterMech.MechMaxEnergy);
        mechHUDManager.UpdatePlayerHP(playerFighter.FighterMech.MechCurrentHP);

        mechHUDManager.UpdatePlayerPilotImage(playerFighter.FighterUIObject);
        combatAnimationManager.SetMechStartingAnimations(playerFighter.FighterMech, CharacterSelect.Player);
        mechSpriteSwapManager.UpdateMechSprites(newPlayerFighter.FighterMech, CharacterSelect.Player);
    }

    private void InitOpponentFighter(FighterDataObject newOpponentFighter)
    {
        opponentFighter = newOpponentFighter;
        opponentFighter.FighterMech.ResetEnergy();

        deckManager.SetOpponentDeck(newOpponentFighter.FighterDeck);

        mechHUDManager.SetOpponentMaxStats(opponentFighter.FighterMech.MechMaxHP, opponentFighter.FighterMech.MechMaxEnergy);
        mechHUDManager.UpdateOpponentPilotImage(opponentFighter.FighterUIObject);

        opponentFighter.FighterMech.DamageWholeMechHP(GameManager.instance.EnemyHealthModifier);
        mechHUDManager.UpdateOpponentHP(opponentFighter.FighterMech.MechCurrentHP);

        aIManager.LoadAIBehaviorModule(opponentFighter.AIBehaviorModule);
        aIManager.LoadAIDialogueModule(opponentFighter.AIDialogueModule);

        combatAnimationManager.SetMechStartingAnimations(opponentFighter.FighterMech, CharacterSelect.Opponent);
        mechSpriteSwapManager.UpdateMechSprites(newOpponentFighter.FighterMech, CharacterSelect.Opponent);
    }
    
    private void StartNewTurnCheck()
    {
        if(hasStartedCombat)
        {
            StartNewTurn();
            hasStartedCombat = false;
        }
    }

    private void StartNewTurn()
    {
        if (hasStartedGame && !hasWon && !hasLost)
        {
            if (GameManager.instance.PlayerWins == 0 && hasNotCompletedIntroDialogue)
            {
                if(playPilotEffectsOnFirstTurn)
                {
                    pilotEffectManager.ManuallyCallPilotEffects();
                }

                hasNotCompletedIntroDialogue = false;
            }

            deckManager.DrawPlayerCard(5 - HandManager.PlayerHand.CharacterHand.Count);
            deckManager.DrawOpponentCard(5 - HandManager.OpponentHand.CharacterHand.Count);

            AddEnergyToMech(CharacterSelect.Opponent, mechEnergyGain + OpponentFighter.FighterMech.MechEnergyGain);
            AddEnergyToMech(CharacterSelect.Player, mechEnergyGain + PlayerFighter.FighterMech.MechEnergyGain);
            OnStartNewTurn?.Invoke();
        }
        
        if(!hasStartedGame && !hasLost && !hasWon)
        {
            AIManager.PlayAIIntroDialogue();
            hasStartedGame = true;
            hasStartedCombat = true;
        }

        if(hasWon)
        {
            statTrackerController.GameOver(true);
            winLossPanel.SetActive(true);
            winPanel.SetActive(true);
            fightButton.SetActive(false);
        }

        if(hasLost)
        {
            statTrackerController.GameOver(false);
            winLossPanel.SetActive(true);
            lossPanel.SetActive(true);
            fightButton.SetActive(false);
        }
    }

    private void CheckForWinLoss()
    {
        if (playerFighter.FighterMech.MechCurrentHP <= 0)
        {
            CombatSequenceManager.ClearCombatQueue();

            AnimationQueueObject newAnimationQueueObject = new AnimationQueueObject(CharacterSelect.Player, AnimationType.Lose, CharacterSelect.Opponent, AnimationType.Win);
            CombatAnimationManager.SetMechAnimation(newAnimationQueueObject);
            
            hasLost = true;
            gameOver = true;
            AIManager.PlayAIWinDialogue();

            return;
        }
        if (opponentFighter.FighterMech.MechCurrentHP <= 0)
        {
            CombatSequenceManager.ClearCombatQueue();

            AnimationQueueObject newAnimationQueueObject = new AnimationQueueObject(CharacterSelect.Player, AnimationType.Win, CharacterSelect.Opponent, AnimationType.Lose);
            CombatAnimationManager.SetMechAnimation(newAnimationQueueObject);

            MechObject playerMech = playerFighter.FighterMech;

            GameManager.instance.PlayerMechController.SetNewPlayerMech(playerMech);
            GameManager.instance.UpdatePlayerAfterFight(playerMech);

            hasWon = true;
            gameOver = true;
            AIManager.PlayAILoseDialogue();


            return;
        }
    }
}
