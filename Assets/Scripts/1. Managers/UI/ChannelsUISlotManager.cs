using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChannelsUISlotManager : BaseSlotManager<CardUIController>
{
    [SerializeField] private List<BaseSlotController<CardUIController>> highChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> midChannels;
    [SerializeField] private List<BaseSlotController<CardUIController>> lowChannels;

    [SerializeField] private BaseSlotController<CardUIController> playerAttackSlotA;
    [SerializeField] private BaseSlotController<CardUIController> playerAttackSlotB;


    [SerializeField] private GameObject SkipASlotButton;
    [Range(.01f, 1f)] [SerializeField] private float channelFadeTimeModifier;


    private CardChannelPairObject cardChannelPairObjectA;
    private CardChannelPairObject cardChannelPairObjectB;

    private bool attackSlotAFilled = false;
    private bool attackSlotBFilled = false;
    private Channels selectedChannel;

    public float ChannelFadeTimeModifier { get => channelFadeTimeModifier; }

    public delegate void onASlotFilled();
    public static event onASlotFilled OnASlotFilled;
    public delegate void onBSlotFilled();
    public static event onBSlotFilled OnBSlotFilled;

    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(item.CardData.CardType == CardType.Attack)
        {
            if(!attackSlotAFilled)
            {
                FillASlot(item, slot);
                return;
            }

            if(attackSlotAFilled && !attackSlotBFilled && playerAttackSlotA.CurrentSlottedItem.CardData.CardType == CardType.Neutral)
            {
                FillBSlot(playerAttackSlotA.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotB);
                FillASlot(item, slot);
                return;
            }
        }

        if (item.CardData.CardType == CardType.Defense)
        {
            if(!attackSlotBFilled)
            {
                FillBSlot(item, slot);
                return;
            }

            if (attackSlotBFilled && !attackSlotAFilled && playerAttackSlotB.CurrentSlottedItem.CardData.CardType == CardType.Neutral)
            {
                FillASlot(playerAttackSlotB.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotB);
                FillBSlot(item, slot);
                return;
            }
        }

        if (item.CardData.CardType == CardType.Neutral)
        {
            if(!attackSlotAFilled)
            {
                FillASlot(item, slot);
                return;
            }

            if (!attackSlotBFilled)
            {
                FillBSlot(item, slot);
                return;
            }
        }


        void FillASlot(CardUIController item, BaseSlotController<CardUIController> slot)
        {
            playerAttackSlotA.CurrentSlottedItem = item;
            item.CardSlotController = playerAttackSlotA;

            item.CardData.SelectedChannels = selectedChannel;

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Player selected " + item.CardData.CardName + " for their A Slot.");

            cardChannelPairObjectA = new CardChannelPairObject(item.CardData, selectedChannel);
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA = cardChannelPairObjectA;

            //Needs to account for ice.
            CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, cardChannelPairObjectA.CardData.EnergyCost, true);

            attackSlotAFilled = true;
            SkipASlotButton.SetActive(false);
            OnASlotFilled?.Invoke();
        }

        void FillBSlot(CardUIController item, BaseSlotController<CardUIController> slot)
        {
            playerAttackSlotB.CurrentSlottedItem = item;
            item.CardSlotController = playerAttackSlotB;

            item.CardData.SelectedChannels = selectedChannel;

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Player selected " + item.CardData.CardName + " for their B Slot.");

            cardChannelPairObjectB = new CardChannelPairObject(item.CardData, selectedChannel);
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairB = cardChannelPairObjectB;

            //Needs to account for ice.
            if (cardChannelPairObjectA != null && cardChannelPairObjectA.CardData != null)
                CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, cardChannelPairObjectA.CardData.EnergyCost + cardChannelPairObjectB.CardData.EnergyCost, true);
            else
                CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, cardChannelPairObjectB.CardData.EnergyCost, true);

            attackSlotBFilled = true;
            OnBSlotFilled?.Invoke();
            return;
        }

        Debug.Log("No slots available in the hand to add a card to.");
    }

    public void SkipASlot()
    {
        if(CombatManager.instance.CanPlayCards)
        {
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA =
            new CardChannelPairObject(null, Channels.None);

            attackSlotAFilled = true;
            OnASlotFilled?.Invoke();

            SkipASlotButton.SetActive(false);
        }
    }

    public override void AddSlotToList(BaseSlotController<CardUIController> newSlot)
    {
        if (slotList == null)
            slotList = new List<BaseSlotController<CardUIController>>();

        slotList.Add(newSlot);
    }

    public override void HandleDrop(PointerEventData eventData, CardUIController newData, BaseSlotController<CardUIController> slot)
    {
        if(ValidateCardChannelSelection(newData, slot))
        {
            //Needs to also account for energy cost + ice
            newData.CardSlotController.SlotManager.RemoveItemFromCollection(newData);
            AddItemToCollection(newData, slot);
        }
    }

    private bool ValidateCardChannelSelection(CardUIController newData, BaseSlotController<CardUIController> slot)
    {
        if (attackSlotAFilled && attackSlotBFilled)
            return false;

        if (newData.CardData.EnergyCost > CombatManager.instance.PlayerFighter.FighterMech.MechCurrentEnergy)
        {
            Debug.Log("Not enough energy to use this card.");
            return false;
        }

        selectedChannel = CheckChannelSlot(slot);

        if (selectedChannel == Channels.None)
        {
            Debug.Log("Channel slot not recognized.");
            return false;
        }

        if (!CardChannelCheck(newData, selectedChannel))
        {
            Debug.Log("Card does not fit into " + selectedChannel + " slot.");
            return false;
        }

        return true;
    }

    public override void RemoveItemFromCollection(CardUIController item)
    {
        if (playerAttackSlotA.CurrentSlottedItem == item)
        {
            playerAttackSlotA.CurrentSlottedItem = null;
            cardChannelPairObjectA = null;
            attackSlotAFilled = false;

            if (playerAttackSlotB.CurrentSlottedItem != null)
                CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, cardChannelPairObjectB.CardData.EnergyCost, true);
            else
                CombatManager.instance.ResetMechEnergyHUD(CharacterSelect.Player);
            return;
        }

        if (playerAttackSlotB.CurrentSlottedItem == item)
        {
            playerAttackSlotB.CurrentSlottedItem = null;
            cardChannelPairObjectB = null;
            attackSlotBFilled = false;

            if (playerAttackSlotA.CurrentSlottedItem != null)
                CombatManager.instance.RemoveEnergyFromMech(CharacterSelect.Player, cardChannelPairObjectA.CardData.EnergyCost, true);
            else
                CombatManager.instance.ResetMechEnergyHUD(CharacterSelect.Player);
            return;
        }
    }

    private void Start()
    {
        CardPlayManager.OnCombatComplete += ResetChannelSelections;
    }

    private void OnDestroy()
    {
        CardPlayManager.OnCombatComplete -= ResetChannelSelections;
    }

    private Channels CheckChannelSlot(BaseSlotController<CardUIController> slotToCheck)
    {
        if (highChannels.Contains(slotToCheck))
            return Channels.High;
        if (midChannels.Contains(slotToCheck))
            return Channels.Mid;
        if (lowChannels.Contains(slotToCheck))
            return Channels.Low;

        return Channels.None;
    }

    private bool CardChannelCheck(CardUIController card, Channels selectedChannel)
    {

        if (card.CardData.PossibleChannels.HasFlag(Channels.All))
            return true;

        //If we're checking the high channel, bitwise check if PossibleChannels results in Channels.High
        if (selectedChannel == Channels.High)
            if (card.CardData.PossibleChannels.HasFlag(Channels.High))
                return true;

        if (selectedChannel == Channels.Mid)
            if (card.CardData.PossibleChannels.HasFlag(Channels.Mid))
                return true;

        if (selectedChannel == Channels.Low)
            if (card.CardData.PossibleChannels.HasFlag(Channels.Low))
                return true;

        return false;
    }

    private void ResetChannelSelections()
    {
        playerAttackSlotA.CurrentSlottedItem = null;
        playerAttackSlotB.CurrentSlottedItem = null;

        attackSlotAFilled = false;
        attackSlotBFilled = false;
    }
}
