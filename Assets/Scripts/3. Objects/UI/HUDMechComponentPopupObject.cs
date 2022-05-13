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
        CombatSequenceManager.OnCombatComplete += CheckPickupUpdate;
    }

    private void OnDestroy()
    {
        CombatSequenceManager.OnCombatComplete -= CheckPickupUpdate;
    }

    private void CheckPickupUpdate()
    {
        if (affectedChannel == Channels.High)
        {
            MechComponentDataObject armsComponent;
            if (mechType == MechSelect.Opponent)
            {
                armsComponent = CombatManager.instance.OpponentFighter.FighterMech.MechArms;
            }
            else
                armsComponent = CombatManager.instance.PlayerFighter.FighterMech.MechArms;

            componentHealth = armsComponent.ComponentCurrentHP;
            componentHealthText.text = Mathf.Clamp(componentHealth, 0, int.MaxValue).ToString();

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
            MechComponentDataObject torsoComponent;
            if (mechType == MechSelect.Opponent)
            {
                torsoComponent = CombatManager.instance.OpponentFighter.FighterMech.MechTorso;
            }
            else
                torsoComponent = CombatManager.instance.PlayerFighter.FighterMech.MechTorso;

            componentHealth = torsoComponent.ComponentCurrentHP;
            componentHealthText.text = Mathf.Clamp(componentHealth, 0, int.MaxValue).ToString();

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
            MechComponentDataObject legsComponent;
            if (mechType == MechSelect.Opponent)
            {
                legsComponent = CombatManager.instance.OpponentFighter.FighterMech.MechLegs;
            }
            else
                legsComponent = CombatManager.instance.PlayerFighter.FighterMech.MechLegs;

            componentHealth = legsComponent.ComponentCurrentHP;
            componentHealthText.text = Mathf.Clamp(componentHealth, 0, int.MaxValue).ToString();

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
