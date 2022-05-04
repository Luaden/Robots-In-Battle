using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Combat Modifiers")]
    [SerializeField] private int mechEnergyGain;
    [SerializeField] private float brokenComponentDamageMultiplier;
    [SerializeField] private float counterDamageMultiplier;
    [SerializeField] private float guardDamageMultiplier;
    [SerializeField] private float acidComponentDamageMultiplier;
    [SerializeField] private int iceChannelEnergyReductionModifier;

    #region Debug
    [Header("Debug / Testing")]
    [SerializeField] private GameObject winLossPanel;
    [SerializeField] private GameObject reloadGameButton;
    [SerializeField] private GameObject loadShoppingButton;
    [SerializeField] private bool narrateCardSelection;
    [SerializeField] private bool narrateCombatChoices;
    [SerializeField] private bool narrateAIDecisionMaking;

    public bool NarrateCardSelection { get => narrateCardSelection; }
    public bool NarrateCombat { get => narrateCombatChoices; }
    public bool NarrateAIDecisionMaking { get => narrateAIDecisionMaking; }
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
    private EffectManager effectManager;

    private FighterDataObject playerFighter;
    private FighterDataObject opponentFighter;
    private Queue<EnergyRemovalObject> energyRemovalQueue;

    private bool canPlayCards = true;

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
    public EffectManager EffectManager { get => effectManager; }

    public int MechEnergyGain { get => mechEnergyGain; }
    public float BrokenCDM { get => brokenComponentDamageMultiplier; }
    public float CounterDamageMultiplier { get => counterDamageMultiplier; }
    public float GuardDamageMultiplier { get => guardDamageMultiplier; }
    public float AcidComponentDamageMultiplier { get => acidComponentDamageMultiplier; }
    public int IceChannelEnergyReductionModifier { get => iceChannelEnergyReductionModifier; }
    public bool CanPlayCards { get => canPlayCards; }

    public delegate void onDestroyScene();
    public static event onDestroyScene OnDestroyScene;

    public delegate void onStartNewTurn();
    public static event onStartNewTurn OnStartNewTurn;

    #region Debug
    public void StartGame()
    {
        StartNewTurn();
    }
    #endregion

    public void RemoveHealthFromMech(DamageMechPairObject damageMechPair)
    {
        if (damageMechPair.CharacterTakingDamage == CharacterSelect.Player)
        {
            int energyCost = damageMechPair.CardChannelPair.CardData.EnergyCost;

            foreach(Channels channel in GetChannelListFromFlags(damageMechPair.GetDamageChannels()))
            {
                switch (channel)
                {
                    case Channels.High:
                        playerFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Player), MechComponent.Arms);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Opponent))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;

                    case Channels.Mid:
                        playerFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Player), MechComponent.Torso);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Opponent))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;

                    case Channels.Low:
                        playerFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Player), MechComponent.Legs);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Opponent))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;                
                }
            }
        }

        if (damageMechPair.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            int energyCost = damageMechPair.CardChannelPair.CardData.EnergyCost;

            foreach (Channels channel in GetChannelListFromFlags(damageMechPair.GetDamageChannels()))
            {
                switch (channel)
                {
                    case Channels.High:
                        opponentFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Opponent), MechComponent.Arms);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Player))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;

                    case Channels.Mid:
                        opponentFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Opponent), MechComponent.Torso);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Opponent))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;

                    case Channels.Low:
                        opponentFighter.FighterMech.DamageComponentHP(
                            effectManager.GetComponentDamageWithModifiers(
                                damageMechPair.GetDamageToDeal(), channel, CharacterSelect.Opponent), MechComponent.Legs);

                        if (effectManager.GetIceElementInChannel(channel, CharacterSelect.Opponent))
                            energyCost *= iceChannelEnergyReductionModifier;
                        break;
                }
            }
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

    public void RemoveMechEnergyWithQueue(EnergyRemovalObject newEnergyRemovalObject)
    {
        energyRemovalQueue.Enqueue(newEnergyRemovalObject);
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
        effectManager = FindObjectOfType<EffectManager>(true);

        energyRemovalQueue = new Queue<EnergyRemovalObject>();
    }

    private void Start()
    {
        CardPlayManager.OnCombatStart += DisableCanPlayCards;
        CardPlayManager.OnCombatComplete += EnableCanPlayCards;
        CardPlayManager.OnCombatComplete += StartNewTurn;
        CombatAnimationManager.OnRoundEnded += RemoveEnergyFromMechs;
        CombatAnimationManager.OnEndedAnimation += CheckForWinLoss;
    }
    private void OnDestroy()
    {
        OnDestroyScene?.Invoke();
        instance = null;
        CardPlayManager.OnCombatStart -= DisableCanPlayCards;
        CardPlayManager.OnCombatComplete -= EnableCanPlayCards;
        CardPlayManager.OnCombatComplete -= StartNewTurn;
        CombatAnimationManager.OnRoundEnded -= RemoveEnergyFromMechs;
        CombatAnimationManager.OnEndedAnimation -= CheckForWinLoss;
    }

    private void RemoveEnergyFromMechs()
    {
        if (energyRemovalQueue.Count > 0)
        {
            EnergyRemovalObject energyRemovalObject = energyRemovalQueue.Dequeue();

            if (energyRemovalObject.firstMech == CharacterSelect.Player)
            {
                playerFighter.FighterMech.MechCurrentEnergy -= energyRemovalObject.firstMechEnergyRemoval;

                if(energyRemovalObject.secondMechEnergyRemoval != 0)
                    opponentFighter.FighterMech.MechCurrentEnergy -= energyRemovalObject.secondMechEnergyRemoval;
            }
            if (energyRemovalObject.firstMech == CharacterSelect.Opponent)
            {
                opponentFighter.FighterMech.MechCurrentEnergy -= energyRemovalObject.firstMechEnergyRemoval;

                if (energyRemovalObject.secondMechEnergyRemoval != 0)
                    playerFighter.FighterMech.MechCurrentEnergy -= energyRemovalObject.secondMechEnergyRemoval;
            }

            mechHUDManager.UpdatePlayerEnergy(playerFighter.FighterMech.MechCurrentEnergy, playerFighter.FighterMech.MechCurrentEnergy, false);
            mechHUDManager.UpdateOpponentEnergy(opponentFighter.FighterMech.MechCurrentEnergy, opponentFighter.FighterMech.MechCurrentEnergy, false);
        }
    }

    private void DisableCanPlayCards()
    {
        canPlayCards = false;
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
    }

    private void InitOpponentFighter(FighterDataObject newOpponentFighter)
    {
        opponentFighter = newOpponentFighter;
        deckManager.SetOpponentDeck(newOpponentFighter.FighterDeck);
        mechHUDManager.SetOpponentMaxStats(opponentFighter.FighterMech.MechMaxHP, opponentFighter.FighterMech.MechMaxEnergy);
    }

    private void StartNewTurn()
    {
        deckManager.DrawPlayerCard(5 - HandManager.PlayerHand.CharacterHand.Count);
        deckManager.DrawOpponentCard(5 - HandManager.OpponentHand.CharacterHand.Count);

        AddEnergyToMech(CharacterSelect.Opponent, mechEnergyGain + OpponentFighter.FighterMech.MechEnergyGain);
        AddEnergyToMech(CharacterSelect.Player, mechEnergyGain + PlayerFighter.FighterMech.MechEnergyGain);

        OnStartNewTurn?.Invoke();
    }

    private void CheckForWinLoss()
    {
        if (playerFighter.FighterMech.MechCurrentHP <= 0)
        {
            CombatAnimationManager.ClearAnimationQueue();

            AnimationQueueObject newAnimationQueueObject = new AnimationQueueObject(CharacterSelect.Player, AnimationType.Lose, CharacterSelect.Opponent, AnimationType.Win);
            CombatAnimationManager.AddAnimationToQueue(newAnimationQueueObject);

            winLossPanel.SetActive(true);
            reloadGameButton.SetActive(true);

            return;
        }
        if (opponentFighter.FighterMech.MechCurrentHP <= 0)
        {
            CombatAnimationManager.ClearAnimationQueue();

            AnimationQueueObject newAnimationQueueObject = new AnimationQueueObject(CharacterSelect.Player, AnimationType.Win, CharacterSelect.Opponent, AnimationType.Lose);
            CombatAnimationManager.AddAnimationToQueue(newAnimationQueueObject);

            MechObject playerMech = playerFighter.FighterMech;

            GameManager.instance.PlayerMechController.SetNewPlayerMech(playerMech);
            GameManager.instance.UpdatePlayerAfterFight(playerMech);

            winLossPanel.SetActive(true);
            loadShoppingButton.SetActive(true);
            return;
        }
    }
}
