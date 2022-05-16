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
            Vector2Int damageShieldPair = primaryData.GetDamageAndShieldWithModifiers();

            if (primaryData.CounterDamage)
                opponentCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                playerGuardedTextObject.SetActive(true);

            if (damageShieldPair.y > 0)
            {
                playerShieldText.text = damageShieldPair.y.ToString();
                playerShieldTextObject.SetActive(true);
            }

            playerDamageText.text = damageShieldPair.x.ToString();
            playerDamageTextObject.SetActive(true);

            playerFloatingTextObject.SetActive(true);
            return;
        }

        if (primaryData.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            Vector2Int damageShieldPair = primaryData.GetDamageAndShieldWithModifiers();

            if (primaryData.CounterDamage)
                playerCounteredTextObject.SetActive(true);

            if (primaryData.GuardDamage)
                opponentGuardedTextObject.SetActive(true);

            if (damageShieldPair.y > 0)
            {
                opponentShieldText.text = damageShieldPair.y.ToString();
                opponentShieldTextObject.SetActive(true);
            }

            opponentDamageText.text = damageShieldPair.x.ToString();
            opponentDamageTextObject.SetActive(true);

            opponentFloatingTextObject.SetActive(true);
        }
    }

    protected override bool ClearedIfEmpty(DamageMechPairObject newData)
    {
        if(newData.CharacterTakingDamage == CharacterSelect.Player)
        {
            playerDamageTextObject.SetActive(false);
            playerShieldTextObject.SetActive(false);
            playerGuardedTextObject.SetActive(false);
            playerCounteredTextObject.SetActive(false);
            playerDamageText.text = string.Empty;
            playerShieldText.text = string.Empty;
        }
        
        if(newData.CharacterTakingDamage == CharacterSelect.Opponent)
        {
            opponentDamageTextObject.SetActive(false);
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
