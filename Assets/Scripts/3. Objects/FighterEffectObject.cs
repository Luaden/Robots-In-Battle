using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEffectObject
{
    private CharacterSelect character;
    private Dictionary<Channels, List<ElementStackObject>> iceAcidStacks;
    private Dictionary<Channels, List<CardEffectObject>> channelDamageBonus;
    private Dictionary<Channels, List<CardEffectObject>> channelDamageReduction;
    private Dictionary<Channels, List<ChannelShieldFalloffObject>> channelShieldFalloff;
    private Dictionary<Channels, int> channelShields;
    private Dictionary<ElementType, int> firePlasmaStacks;
    private Dictionary<CardKeyWord, List<CardEffectObject>> keywordDuration;
    private Dictionary<CardCategory, List<CardEffectObject>> cardCategoryDamageBonus;

    public Dictionary<Channels, List<ElementStackObject>> IceAcidStacks { get => iceAcidStacks; }
    public Dictionary<ElementType, int> FirePlasmaStacks { get => firePlasmaStacks; }
    public Dictionary<CardCategory, List<CardEffectObject>> CardCategoryDamageBonus { get => cardCategoryDamageBonus; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamageBonus { get => channelDamageBonus; }
    public Dictionary<Channels, List<CardEffectObject>> ChannelDamageReduction { get => channelDamageReduction; }
    public Dictionary<Channels, int> ChannelShields { get => channelShields; }
    public Dictionary<Channels, List<ChannelShieldFalloffObject>> ChannelShieldsFalloff { get => channelShieldFalloff; }
    public Dictionary<CardKeyWord, List<CardEffectObject>> KeyWordDuration { get => keywordDuration; }

    public FighterEffectObject(CharacterSelect character)
    {
        this.character = character;
        iceAcidStacks = new Dictionary<Channels, List<ElementStackObject>>();
        firePlasmaStacks = new Dictionary<ElementType, int>();
        channelDamageBonus = new Dictionary<Channels, List<CardEffectObject>>();
        channelDamageReduction = new Dictionary<Channels, List<CardEffectObject>>();
        channelShieldFalloff = new Dictionary<Channels, List<ChannelShieldFalloffObject>>();
        channelShields = new Dictionary<Channels, int>();
        keywordDuration = new Dictionary<CardKeyWord, List<CardEffectObject>>();
        cardCategoryDamageBonus = new Dictionary<CardCategory, List<CardEffectObject>>();
    }

    public void IncrementFighterEffects()
    {
        ReduceElementStacks(IceAcidStacks);
        ReduceElementStacks(FirePlasmaStacks);
        ReduceCategoryDamageBonusEffects(CardCategoryDamageBonus);
        ReduceChannelEffects(ChannelDamageBonus);
        ReduceKeyWordEffects(KeyWordDuration);
        ReduceChannelEffects(ChannelDamageReduction);
        ReduceChannelShieldsWithFalloff(ChannelShieldsFalloff);
    }

    private void ReduceChannelShieldsWithFalloff(Dictionary<Channels, List<ChannelShieldFalloffObject>> channelShieldsFalloff)
    {
        List<KeyValuePair<Channels, List<ChannelShieldFalloffObject>>> shieldsKVPairList = new List<KeyValuePair<Channels, List<ChannelShieldFalloffObject>>>();
        List<KeyValuePair<Channels, List<ChannelShieldFalloffObject>>> shieldsKVPairRemovalList = new List<KeyValuePair<Channels, List<ChannelShieldFalloffObject>>>();

        //Foreach KeyValue pair in the dictionary
        foreach (KeyValuePair<Channels, List<ChannelShieldFalloffObject>> shieldFalloffKVPair in channelShieldFalloff)
        {
            List<ChannelShieldFalloffObject> removalObjects = new List<ChannelShieldFalloffObject>();

            //Foreach ChannelShieldFalloffObject in each KeyValue pair
            foreach (ChannelShieldFalloffObject channelShieldFalloffObject in shieldFalloffKVPair.Value)
            {
                //Add shields
                AddShields(shieldFalloffKVPair.Key, channelShieldFalloffObject.StartingShieldPerTurn);

                //Shields falloff
                channelShieldFalloffObject.StartingShieldPerTurn -= channelShieldFalloffObject.FalloffPerTurn;

                //If the shield gain is less than or equal to zero, add it to the removal list
                if (channelShieldFalloffObject.StartingShieldPerTurn <= 0)
                    removalObjects.Add(channelShieldFalloffObject);
            }

            //Foreach item in the removal list, remove it from the KeyValue pair list (value)
            foreach (ChannelShieldFalloffObject channelShieldFalloffObject in removalObjects)
                shieldFalloffKVPair.Value.Remove(channelShieldFalloffObject);

            //If the KeyValuePair no longer has any Channel Shield Fall Off Objects, add it to the KeyValuePair removal list
            if (channelShieldFalloff[shieldFalloffKVPair.Key].Count == 0)
                shieldsKVPairRemovalList.Add(shieldFalloffKVPair);
        }

        //Foreach item in the KeyValuePair removal list, remove the key from the dictionary because it's empty now.
        foreach (KeyValuePair<Channels, List<ChannelShieldFalloffObject>> shieldKVPair in shieldsKVPairRemovalList)
            channelShieldFalloff.Remove(shieldKVPair.Key);
    }

    private void AddShields(Channels channel, int shieldsToGain)
    {
        int oldShield;

        if (ChannelShields.TryGetValue(channel, out oldShield))
            ChannelShields[channel] =
                ChannelShields[channel] + shieldsToGain;
        else
            ChannelShields.Add(channel, shieldsToGain);
    }

    private void ReduceElementStacks(Dictionary<Channels, List<ElementStackObject>> elementDict)
    {
        List<KeyValuePair<Channels, List<ElementStackObject>>> elementKVPairList = new List<KeyValuePair<Channels, List<ElementStackObject>>>();

        foreach (KeyValuePair<Channels, List<ElementStackObject>> pair in elementDict)
        {
            List<ElementStackObject> newElementStackList = new List<ElementStackObject>();

            foreach (ElementStackObject elementStack in elementDict[pair.Key])
            {
                ElementStackObject newStacks = new ElementStackObject();

                newStacks.ElementType = elementStack.ElementType;
                newStacks.ElementStacks = elementStack.ElementStacks - 1;

                if (newStacks.ElementStacks > 0)
                    newElementStackList.Add(newStacks);
            }

            elementDict[pair.Key] = newElementStackList;
        }
    }

    private void ReduceElementStacks(Dictionary<ElementType, int> elementDict)
    {
        Dictionary<ElementType, int> newElementDict = new Dictionary<ElementType, int>();

        int newStacks;

        foreach (KeyValuePair<ElementType, int> pair in elementDict)
        {
            if (pair.Key == ElementType.Fire)
            {
                DamageEffectMechPairObject damageEffectMechPair = new DamageEffectMechPairObject();
                damageEffectMechPair.damageToTake = pair.Value;
                damageEffectMechPair.characterToTakeDamage = character;
                CombatManager.instance.RemoveHealthFromMech(damageEffectMechPair);

                newStacks = pair.Value - 1;

                if (newStacks > 0)
                    newElementDict.Add(pair.Key, newStacks);
            }
            if(pair.Key == ElementType.Plasma)
            {
                EnergyRemovalObject newEnergyToRemove = new EnergyRemovalObject();
                newEnergyToRemove.firstMech = character;
                newEnergyToRemove.firstMechEnergyRemoval = pair.Value;

                CombatManager.instance.RemoveMechEnergyWithQueue(newEnergyToRemove);
                CombatManager.instance.AddEnergyToMech(character == CharacterSelect.Player ? CharacterSelect.Opponent : CharacterSelect.Player, pair.Value);
            }
        }

        firePlasmaStacks = new Dictionary<ElementType, int>(newElementDict);
    }

    private void ReduceCategoryDamageBonusEffects(Dictionary<CardCategory, List<CardEffectObject>> cardCategoryDict)
    {
        List<KeyValuePair<CardCategory, List<CardEffectObject>>> categoryKVPairList = new List<KeyValuePair<CardCategory, List<CardEffectObject>>>();
        List<CardEffectObject> effectList = new List<CardEffectObject>();

        foreach (KeyValuePair<CardCategory, List<CardEffectObject>> pair in cardCategoryDict)
        {
            if (pair.Value == null)
                continue;

            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    effectList.Add(effect);
            }

            foreach (CardEffectObject effect in effectList)
                pair.Value.Remove(effect);

            if (pair.Value.Count == 0)
                categoryKVPairList.Add(pair);
        }

        foreach (KeyValuePair<CardCategory, List<CardEffectObject>> pair in categoryKVPairList)
            cardCategoryDict.Remove(pair.Key);
    }

    private void ReduceChannelEffects(Dictionary<Channels, List<CardEffectObject>> channelDamageDict)
    {
        List<KeyValuePair<Channels, List<CardEffectObject>>> channelsKVPairList = new List<KeyValuePair<Channels, List<CardEffectObject>>>();
        List<CardEffectObject> channelEffectList = new List<CardEffectObject>();

        foreach (KeyValuePair<Channels, List<CardEffectObject>> pair in channelDamageDict)
        {
            if (pair.Value == null)
                continue;

            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    channelEffectList.Add(effect);
            }

            foreach (CardEffectObject effect in channelEffectList)
                pair.Value.Remove(effect);

            if (pair.Value.Count == 0)
                channelsKVPairList.Add(pair);
        }

        foreach (KeyValuePair<Channels, List<CardEffectObject>> pair in channelsKVPairList)
            channelDamageDict.Remove(pair.Key);

    }

    private void ReduceKeyWordEffects(Dictionary<CardKeyWord, List<CardEffectObject>> keyWordDict)
    {
        List<KeyValuePair<CardKeyWord, List<CardEffectObject>>> keyWordKVPair = new List<KeyValuePair<CardKeyWord, List<CardEffectObject>>>();
        List<CardEffectObject> keyWordEffectList = new List<CardEffectObject>();

        foreach (KeyValuePair<CardKeyWord, List<CardEffectObject>> pair in keyWordDict)
        {
            if (pair.Value == null)
                continue;

            foreach (CardEffectObject effect in pair.Value)
            {
                effect.CurrentTurn++;

                if (effect.CurrentTurn >= effect.EffectDuration)
                    keyWordEffectList.Add(effect);
            }

            foreach (CardEffectObject effect in keyWordEffectList)
                pair.Value.Remove(effect);

            if (pair.Value.Count == 0)
                keyWordKVPair.Add(pair);
        }

        foreach (KeyValuePair<CardKeyWord, List<CardEffectObject>> pair in keyWordKVPair)
            keyWordDict.Remove(pair.Key);
    }
}
