using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDFloatingDamagePopupController : BaseUIElement<DamageMechPairObject>
{
    [SerializeField] private GameObject playerFloatingTextObject;
    [SerializeField] private GameObject playerDamageTextObject;
    [SerializeField] private GameObject playerShieldTextObject;
    [SerializeField] private GameObject playerGuardedTextObject;
    [SerializeField] private GameObject playerCounteredTextObject;
    [SerializeField] private TMP_Text playerShieldText;
    [SerializeField] private TMP_Text playerDamageText;

    [SerializeField] private GameObject opponentFloatingTextObject;
    [SerializeField] private GameObject opponentDamageTextObject;
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
            if (primaryData.CounterDamage)
                opponentCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                playerGuardedTextObject.SetActive(true);

            if (primaryData.GetShieldAmount() > 0)
            {
                playerShieldText.text = primaryData.GetShieldAmount().ToString();
                playerShieldTextObject.SetActive(true);
            }

            playerDamageText.text = primaryData.GetDamageToDeal().ToString();
            playerDamageTextObject.SetActive(true);

            playerFloatingTextObject.SetActive(true);
        }

        if (primaryData.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            if (primaryData.CounterDamage)
                playerCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                opponentGuardedTextObject.SetActive(true);

            if (primaryData.GetShieldAmount() > 0)
            {
                opponentShieldText.text = primaryData.GetShieldAmount().ToString();
                opponentShieldTextObject.SetActive(true);
            }

            opponentDamageText.text = primaryData.GetDamageToDeal().ToString();
            opponentDamageTextObject.SetActive(true);

            opponentFloatingTextObject.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(DamageMechPairObject newData)
    {
        if (newData == null || newData.CardCharacterPairA.cardChannelPair.CardData.BaseDamage == 0)
        {
            playerDamageTextObject.SetActive(false);
            playerGuardedTextObject.SetActive(false);
            playerCounteredTextObject.SetActive(false);
            playerDamageText.text = string.Empty;
            playerShieldText.text = string.Empty;

            opponentDamageTextObject.SetActive(false);
            opponentGuardedTextObject.SetActive(false);
            opponentCounteredTextObject.SetActive(false);
            opponentDamageText.text = string.Empty;
            opponentShieldText.text = string.Empty;
            return true;
        }

        return false;
    }
}
