using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDFloatingDamagePopupController : BaseUIElement<DamageMechPairObject>
{
    [SerializeField] private GameObject playerFloatingTextObject;
    [SerializeField] private GameObject playerDamageTextObject;
    [SerializeField] private GameObject playerBonusDamageTextObject;
    [SerializeField] private GameObject playerShieldTextObject;
    [SerializeField] private GameObject playerGuardedTextObject;
    [SerializeField] private GameObject playerCounteredTextObject;
    [SerializeField] private TMP_Text playerShieldText;
    [SerializeField] private TMP_Text playerDamageText;

    [SerializeField] private GameObject opponentFloatingTextObject;
    [SerializeField] private GameObject opponentDamageTextObject;
    [SerializeField] private GameObject opponentBonusDamageTextObject;
    [SerializeField] private GameObject opponentShieldTextObject;
    [SerializeField] private GameObject opponentGuardedTextObject;
    [SerializeField] private GameObject opponentCounteredTextObject;
    [SerializeField] private TMP_Text opponentShieldText;
    [SerializeField] private TMP_Text opponentDamageText;

    public override void UpdateUI(DamageMechPairObject primaryData)
    {
        if (ClearedIfEmpty(primaryData))
            return;

        if(primaryData.CharacterTakingDamage == CharacterSelect.Player)
        {
            Vector2Int damageShieldPair = primaryData.GetDamageAndShieldWithModifiers();
            
            int bonusDamage = 0;

            switch (primaryData.CardCharacterPairA.cardChannelPair.CardData.SelectedChannels)
            {
                case Channels.High:
                    bonusDamage = CombatManager.instance.PlayerFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Head);
                    break;
                case Channels.Mid:
                    bonusDamage = CombatManager.instance.PlayerFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Torso);
                    break;
                case Channels.Low:
                    bonusDamage = CombatManager.instance.PlayerFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Legs);
                    break;
            }

            if(bonusDamage > damageShieldPair.x)
            {
                playerDamageText.text = (bonusDamage).ToString();
                playerBonusDamageTextObject.SetActive(true);
            }
            else
            {
                if(damageShieldPair.x > 0)
                {
                    playerDamageText.text = damageShieldPair.x.ToString();
                    playerDamageTextObject.SetActive(true);
                }                
            }

            if (primaryData.CounterDamage)
                opponentCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                playerGuardedTextObject.SetActive(true);

            if (damageShieldPair.y > 0)
            {
                playerShieldText.text = damageShieldPair.y.ToString();
                playerShieldTextObject.SetActive(true);
            }

            playerFloatingTextObject.SetActive(true);
            return;
        }

        if (primaryData.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            Vector2Int damageShieldPair = primaryData.GetDamageAndShieldWithModifiers();

            int bonusDamage = 0;

            switch (primaryData.CardCharacterPairA.cardChannelPair.CardData.SelectedChannels)
            {
                case Channels.High:
                    bonusDamage = CombatManager.instance.OpponentFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Head);
                    break;
                case Channels.Mid:
                    bonusDamage = CombatManager.instance.OpponentFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Torso);
                    break;
                case Channels.Low:
                    bonusDamage = CombatManager.instance.OpponentFighter.FighterMech.GetDamageWithBonus(damageShieldPair.x, MechComponent.Legs);
                    break;
            }

            if (bonusDamage > damageShieldPair.x)
            {
                Debug.Log("Bonus damage: " + bonusDamage + " was greater than initial estimate: " + damageShieldPair.x);
                opponentDamageText.text = (bonusDamage).ToString();
                opponentBonusDamageTextObject.SetActive(true);
            }
            else
            {
                if(damageShieldPair.x > 0)
                {
                    opponentDamageText.text = damageShieldPair.x.ToString();
                    opponentDamageTextObject.SetActive(true);
                }
            }

            if (primaryData.CounterDamage)
                playerCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                opponentGuardedTextObject.SetActive(true);

            if (damageShieldPair.y > 0)
            {
                opponentShieldText.text = damageShieldPair.y.ToString();
                opponentShieldTextObject.SetActive(true);
            }

            opponentFloatingTextObject.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(DamageMechPairObject newData)
    {
        if(newData.CharacterTakingDamage == CharacterSelect.Player)
        {
            playerDamageTextObject.SetActive(false);
            playerBonusDamageTextObject.SetActive(false);
            playerShieldTextObject.SetActive(false);
            playerGuardedTextObject.SetActive(false);
            playerCounteredTextObject.SetActive(false);
            playerDamageText.text = string.Empty;
            playerShieldText.text = string.Empty;
        }
        
        if(newData.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            opponentDamageTextObject.SetActive(false);
            opponentBonusDamageTextObject.SetActive(false);
            opponentShieldTextObject.SetActive(false);
            opponentGuardedTextObject.SetActive(false);
            opponentCounteredTextObject.SetActive(false);
            opponentDamageText.text = string.Empty;
            opponentShieldText.text = string.Empty;
        }
        

        if (newData == null || newData.CardCharacterPairA.cardChannelPair.CardData.BaseDamage == 0)
            return true;

        return false;
    }
}
