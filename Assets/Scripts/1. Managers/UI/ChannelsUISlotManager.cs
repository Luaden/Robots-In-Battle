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

    [Range(.01f, 2f)] [SerializeField] private float channelFadeTimeModifier;


    private CardChannelPairObject cardChannelPairObjectA;
    private CardChannelPairObject cardChannelPairObjectB;
    private int aCardEnergyCost = 0;
    private int bCardEnergyCost = 0;

    private bool attackSlotAFilled = false;
    private bool attackSlotBFilled = false;
    private Channels selectedChannel;

    public float ChannelFadeTimeModifier { get => channelFadeTimeModifier; }

    public delegate void onSlotFilled();
    public static event onSlotFilled OnSlotFilled;


    public override void AddItemToCollection(CardUIController item, BaseSlotController<CardUIController> slot)
    {
        if(item.CardData.CardType == CardType.Attack)
        {
            if(!attackSlotAFilled)
            {
                FillASlot(item, slot);
                return;
            }

            else if(attackSlotAFilled && playerAttackSlotA.CurrentSlottedItem == item)
            {
                FillASlot(item, slot);
                return;
            }

            else if (attackSlotAFilled && !attackSlotBFilled && playerAttackSlotA.CurrentSlottedItem.CardData.CardType == CardType.Neutral)
            {
                FillBSlot(playerAttackSlotA.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotB, false);
                FillASlot(item, slot);
                return;
            }
            else if(attackSlotAFilled)
            {
                CombatManager.instance.PlayerHandSlotManager.AddItemToCollection(playerAttackSlotA.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotA);
                FillASlot(item, slot);
                return;
            }
            else
                return;
        }

        if (item.CardData.CardType == CardType.Defense)
        {
            if(!attackSlotBFilled)
            {
                FillBSlot(item, slot);
                return;
            }

            else if (attackSlotBFilled && playerAttackSlotB.CurrentSlottedItem == item)
            {
                FillBSlot(item, slot);
                return;
            }

            else if (attackSlotBFilled && !attackSlotAFilled && playerAttackSlotB.CurrentSlottedItem.CardData.CardType == CardType.Neutral)
            {
                FillASlot(playerAttackSlotB.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotB, false);
                FillBSlot(item, slot);
                return;
            }
            else if (attackSlotBFilled)
            {
                CombatManager.instance.PlayerHandSlotManager.AddItemToCollection(playerAttackSlotB.CurrentSlottedItem.CardData.CardUIController, playerAttackSlotB);
                FillBSlot(item, slot);
                return;
            }
            else
                return;
        }

        if (item.CardData.CardType == CardType.Neutral)
        {
            if(!attackSlotAFilled)
            {
                FillASlot(item, slot);
                return;
            }
            else if(attackSlotAFilled && playerAttackSlotA.CurrentSlottedItem == item)
            {
                FillASlot(item, slot);
                return;
            }
            else if (!attackSlotBFilled)
            {
                FillBSlot(item, slot);
                return;
            }
            else if(attackSlotBFilled && playerAttackSlotB.CurrentSlottedItem == item)
            {
                FillBSlot(item, slot);
            }
            else
                return;
        }


        void FillASlot(CardUIController item, BaseSlotController<CardUIController> slot, bool newSelectedChannel = true)
        {
            item.CardSlotController.SlotManager.RemoveItemFromCollection(item);

            playerAttackSlotA.CurrentSlottedItem = item;
            item.CardSlotController = playerAttackSlotA;

            if(newSelectedChannel)
                item.CardData.SelectedChannels = selectedChannel;

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Player selected " + item.CardData.CardName + " for their A Slot.");

            cardChannelPairObjectA = new CardChannelPairObject(item.CardData, selectedChannel);
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA = cardChannelPairObjectA;

            aCardEnergyCost = cardChannelPairObjectA.CardData.EnergyCost;

            if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectA.CardChannel, CharacterSelect.Player))
            {
                aCardEnergyCost = Mathf.RoundToInt(aCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
            }

            if (cardChannelPairObjectB != null && cardChannelPairObjectB.CardData != null)
            {
                bCardEnergyCost = cardChannelPairObjectB.CardData.EnergyCost;

                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectB.CardChannel, CharacterSelect.Player))
                    bCardEnergyCost = Mathf.RoundToInt(bCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);

                    CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, aCardEnergyCost + bCardEnergyCost);
            }
            else
            {
                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectA.CardChannel, CharacterSelect.Player))
                    aCardEnergyCost = Mathf.RoundToInt(aCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);

                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, aCardEnergyCost);
            }

            attackSlotAFilled = true;
            OnSlotFilled?.Invoke();
        }

        void FillBSlot(CardUIController item, BaseSlotController<CardUIController> slot, bool newSelectedChannel = true)
        {
            item.CardSlotController.SlotManager.RemoveItemFromCollection(item);

            playerAttackSlotB.CurrentSlottedItem = item;
            item.CardSlotController = playerAttackSlotB;

            if(newSelectedChannel)
                item.CardData.SelectedChannels = selectedChannel;

            if (CombatManager.instance.NarrateCardSelection)
                Debug.Log("Player selected " + item.CardData.CardName + " for their B Slot.");

            cardChannelPairObjectB = new CardChannelPairObject(item.CardData, selectedChannel);
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairB = cardChannelPairObjectB;

            bCardEnergyCost = cardChannelPairObjectB.CardData.EnergyCost;

            if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectB.CardChannel, CharacterSelect.Player))
            {
                bCardEnergyCost = Mathf.RoundToInt(bCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);
            }

            if (cardChannelPairObjectA != null && cardChannelPairObjectA.CardData != null)
            {
                aCardEnergyCost = cardChannelPairObjectA.CardData.EnergyCost;

                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectA.CardChannel, CharacterSelect.Player))
                    aCardEnergyCost = Mathf.RoundToInt(aCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);

                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, aCardEnergyCost + bCardEnergyCost);
            }
            else
            {
                if (CombatManager.instance.CombatEffectManager.GetIceElementInChannel(cardChannelPairObjectB.CardChannel, CharacterSelect.Player))
                    bCardEnergyCost = Mathf.RoundToInt(bCardEnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier);

                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, bCardEnergyCost);
            }

            attackSlotBFilled = true;
            OnSlotFilled?.Invoke();
            return;
        }

        Debug.Log("No slots available in the hand to add a card to.");
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
            AddItemToCollection(newData, slot);

        CombatManager.instance.CardClickController.ClearClickedCard();
    }

    private bool ValidateCardChannelSelection(CardUIController newData, BaseSlotController<CardUIController> slot)
    {
        //if (attackSlotAFilled && attackSlotBFilled)
        //    if(playerAttackSlotA.CurrentSlottedItem != newData && playerAttackSlotB.CurrentSlottedItem != newData)
        //    {
        //        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.PlayerFighter.FighterName, 
        //            "The slots for " + newData.CardData.CardName + " are already filled.", CharacterSelect.Player);
        //        return false;
        //    }

        int bonusEnergyCost = 0;

        if (aCardEnergyCost > 0)
            bonusEnergyCost += aCardEnergyCost;
        if (bCardEnergyCost > 0)
            bonusEnergyCost += bCardEnergyCost;

        if (newData.CardData.AffectedChannels == AffectedChannels.AllPossibleChannels)
        {
            foreach(Channels channel in CombatManager.instance.GetChannelListFromFlags(newData.CardData.PossibleChannels))
            {
                if(CombatManager.instance.CombatEffectManager.GetIceElementInChannel(channel, CharacterSelect.Player))
                    if(bonusEnergyCost + (newData.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier) >
                    CombatManager.instance.PlayerFighter.FighterMech.MechCurrentEnergy)
                    {
                        CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.PlayerFighter.FighterName, 
                            "I don't have enough energy to play that card!", CharacterSelect.Player);
                        return false;
                    }
            }
        }

        if(newData.CardData.AffectedChannels == AffectedChannels.SelectedChannel)
        {
            if(CombatManager.instance.CombatEffectManager.GetIceElementInChannel(CheckChannelSlot(slot), CharacterSelect.Player))
                if(bonusEnergyCost + (newData.CardData.EnergyCost * CombatManager.instance.IceChannelEnergyReductionModifier) >
                    CombatManager.instance.PlayerFighter.FighterMech.MechCurrentEnergy)
                {
                    CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.PlayerFighter.FighterName, 
                        "I don't have enough energy to play that card!", CharacterSelect.Player);
                    return false;
                }
        }

        if (bonusEnergyCost + newData.CardData.EnergyCost > CombatManager.instance.PlayerFighter.FighterMech.MechCurrentEnergy)
        {
            CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.PlayerFighter.FighterName, 
                "I don't have enough energy to play that card!", CharacterSelect.Player);
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
            CombatManager.instance.PopupUIManager.HandlePopup(CombatManager.instance.PlayerFighter.FighterName, 
                newData.CardData.CardName + " doesn't fit into that channel.", CharacterSelect.Player);
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
            aCardEnergyCost = 0;
            item.UpdateSelectedChannel(item.CardData.PossibleChannels);

            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairA = null;

            if (playerAttackSlotB.CurrentSlottedItem != null)
                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, cardChannelPairObjectB.CardData.EnergyCost);
            else
                CombatManager.instance.ResetMechEnergyHUD(CharacterSelect.Player);
            return;
        }

        if (playerAttackSlotB.CurrentSlottedItem == item)
        {
            playerAttackSlotB.CurrentSlottedItem = null;
            cardChannelPairObjectB = null;
            attackSlotBFilled = false;
            bCardEnergyCost = 0;
            item.UpdateSelectedChannel(item.CardData.PossibleChannels);
            
            CombatManager.instance.CardPlayManager.PlayerAttackPlan.cardChannelPairB = null;

            if (playerAttackSlotA.CurrentSlottedItem != null)
                CombatManager.instance.PreviewEnergyConsumption(CharacterSelect.Player, cardChannelPairObjectA.CardData.EnergyCost);
            else
                CombatManager.instance.ResetMechEnergyHUD(CharacterSelect.Player);
            return;
        }
    }

    private void Start()
    {
        CombatSequenceManager.OnCombatComplete += ResetChannelSelections;
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= ResetChannelSelections;
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

        aCardEnergyCost = 0;
        bCardEnergyCost = 0;

        attackSlotAFilled = false;
        attackSlotBFilled = false;
    }
}
