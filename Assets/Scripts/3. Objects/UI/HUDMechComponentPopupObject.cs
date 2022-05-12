using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HUDMechComponentPopupObject : MonoBehaviour
{
    [SerializeField] private MechSelect mechType;
    [SerializeField] private Channels affectedChannel;
    [SerializeField] private TMP_Text componentHealthText;
    [SerializeField] private TMP_Text elementTypeText;

    private int componentHealth;

    private void Start()
    {
        CardUIController.OnPickUp += CheckPickupUpdate;
        CheckPickupUpdate(affectedChannel);
    }

    private void OnDestroy()
    {
        CardUIController.OnPickUp -= CheckPickupUpdate;
    }

    private void CheckPickupUpdate(Channels checkChannel)
    {
        if (!checkChannel.HasFlag(affectedChannel))
            return;

        if (affectedChannel == Channels.High)
        {
            MechComponentDataObject armsComponent = CombatManager.instance.OpponentFighter.FighterMech.MechArms;
            componentHealth = armsComponent.ComponentCurrentHP;
            componentHealthText.text = componentHealth.ToString();

            switch (armsComponent.ComponentElement)
            {
                case ElementType.None:
                    elementTypeText.text = "None.";
                    break;
                case ElementType.Fire:
                    elementTypeText.text = "Fire.";
                    break;
                case ElementType.Ice:
                    elementTypeText.text = "Ice.";
                    break;
                case ElementType.Plasma:
                    elementTypeText.text = "Plasma.";
                    break;
                case ElementType.Acid:
                    elementTypeText.text = "Acid.";
                    break;
                case ElementType.Void:
                    elementTypeText.text = "Void.";
                    break;
            }
        }

        if (affectedChannel == Channels.Mid)
        {
            MechComponentDataObject torsoComponent = CombatManager.instance.OpponentFighter.FighterMech.MechTorso;
            componentHealth = torsoComponent.ComponentCurrentHP;
            componentHealthText.text = componentHealth.ToString();

            switch (torsoComponent.ComponentElement)
            {
                case ElementType.None:
                    elementTypeText.text = "None.";
                    break;
                case ElementType.Fire:
                    elementTypeText.text = "Fire.";
                    break;
                case ElementType.Ice:
                    elementTypeText.text = "Ice.";
                    break;
                case ElementType.Plasma:
                    elementTypeText.text = "Plasma.";
                    break;
                case ElementType.Acid:
                    elementTypeText.text = "Acid.";
                    break;
                case ElementType.Void:
                    elementTypeText.text = "Void.";
                    break;
            }
        }

        if (affectedChannel == Channels.Low)
        {
            MechComponentDataObject legsComponent = CombatManager.instance.OpponentFighter.FighterMech.MechLegs;
            componentHealth = legsComponent.ComponentCurrentHP;
            componentHealthText.text = componentHealth.ToString();

            switch (legsComponent.ComponentElement)
            {
                case ElementType.None:
                    elementTypeText.text = "None.";
                    break;
                case ElementType.Fire:
                    elementTypeText.text = "Fire.";
                    break;
                case ElementType.Ice:
                    elementTypeText.text = "Ice.";
                    break;
                case ElementType.Plasma:
                    elementTypeText.text = "Plasma.";
                    break;
                case ElementType.Acid:
                    elementTypeText.text = "Acid.";
                    break;
                case ElementType.Void:
                    elementTypeText.text = "Void.";
                    break;
            }
        }
    }
}
