using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField] private ChannelDamageBuffController playerChannelDamageBuffController;
    [SerializeField] private ChannelDamageBuffController opponentChannelDamageBuffController;
    [SerializeField] private ChannelShieldsController playerChannelShieldsController;
    [SerializeField] private ChannelShieldsController opponentChannelShieldsController;
    [SerializeField] private ChannelShieldsFalloffController playerChannelShieldsFalloffController;
    [SerializeField] private ChannelShieldsFalloffController opponentChannelShieldsFalloffController;
    [SerializeField] private ChannelElementBuffController playerChannelElementBuffController;
    [SerializeField] private ChannelElementBuffController opponentChannelElementBuffController;
    [SerializeField] private GlobalElementBuffController playerGlobalElementBuffController;
    [SerializeField] private GlobalElementBuffController opponentGlobalElementBuffController;
    [SerializeField] private GlobalCategoryBuffController playerGlobalCategoryBuffController;
    [SerializeField] private GlobalCategoryBuffController opponentGlobalCategoryBuffController;
    [SerializeField] private GlobalKeyWordBuffController playerGlobalKeyWordBuffController;
    [SerializeField] private GlobalKeyWordBuffController opponentGlobalKeyWordBuffController;

    public void UpdateChannelDamageBuffs(CharacterSelect character, Dictionary<Channels, List<CardEffectObject>> channelEffectDict)
    {
        if(character == CharacterSelect.Player)
            playerChannelDamageBuffController.UpdateUI(channelEffectDict);

        if (character == CharacterSelect.Opponent)
            opponentChannelDamageBuffController.UpdateUI(channelEffectDict);
    }

    public void UpdateChannelShields(CharacterSelect character, Dictionary<Channels, int> channelIntShieldDict)
    {
        if (character == CharacterSelect.Player)
            playerChannelShieldsController.UpdateUI(channelIntShieldDict);

        if (character == CharacterSelect.Opponent)
            opponentChannelShieldsController.UpdateUI(channelIntShieldDict);
    }

    public void UpdateChannelShieldsFalloff(CharacterSelect character, Dictionary<Channels, List<ChannelShieldFalloffObject>> channelShieldFalloffDict)
    {
        if (character == CharacterSelect.Player)
            playerChannelShieldsFalloffController.UpdateUI(channelShieldFalloffDict);

        if (character == CharacterSelect.Opponent)
            opponentChannelShieldsFalloffController.UpdateUI(channelShieldFalloffDict);
    }

    public void UpdateChannelElementStacks(CharacterSelect character, Dictionary<Channels, List<ElementStackObject>> elementStackDict)
    {
        if (character == CharacterSelect.Player)
            playerChannelElementBuffController.UpdateUI(elementStackDict);

        if (character == CharacterSelect.Opponent)
            opponentChannelElementBuffController.UpdateUI(elementStackDict);
    }

    public void UpdateGlobalElementStacks(CharacterSelect character, Dictionary<ElementType, int> elementStackDict)
    {
        if (character == CharacterSelect.Player)
            playerGlobalElementBuffController.UpdateUI(elementStackDict);

        if (character == CharacterSelect.Opponent)
            opponentGlobalElementBuffController.UpdateUI(elementStackDict);
    }

    public void UpdateGlobalCategoryDamageBuffs(CharacterSelect character, Dictionary<CardCategory, List<CardEffectObject>> categoryDamageBuffs)
    {
        if (character == CharacterSelect.Player)
            playerGlobalCategoryBuffController.UpdateUI(categoryDamageBuffs);

        if (character == CharacterSelect.Opponent)
            opponentGlobalCategoryBuffController.UpdateUI(categoryDamageBuffs);
    }

    public void UpdateGlobalKeyWordDamageBuffs(CharacterSelect character, Dictionary<CardKeyWord, List<CardEffectObject>> keywordDurationDict)
    {
        if (character == CharacterSelect.Player)
            playerGlobalKeyWordBuffController.UpdateUI(keywordDurationDict);

        if (character == CharacterSelect.Opponent)
            opponentGlobalKeyWordBuffController.UpdateUI(keywordDurationDict);
    }
}
