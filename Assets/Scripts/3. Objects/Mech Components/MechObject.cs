using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechObject
{
    private int mechMaxHP;
    private int mechCurrentHP;
    private int mechMaxEnergy;
    private int mechCurrentEnergy;
    private int mechEnergyGain;
    private MechComponentDataObject mechHead;
    private MechComponentDataObject mechTorso;
    private MechComponentDataObject mechArms;
    private MechComponentDataObject mechLegs;

    public MechComponentDataObject MechHead { get => mechHead; }
    public MechComponentDataObject MechTorso { get => mechTorso; }
    public MechComponentDataObject MechArms { get => mechArms; }
    public MechComponentDataObject MechLegs { get => mechLegs; }
    public int MechMaxHP { get => mechMaxHP; }
    public int MechMaxEnergy { get => mechMaxEnergy; }
    public int MechEnergyGain { get => mechEnergyGain; }
    public int MechCurrentHP { get => mechCurrentHP; set => mechCurrentHP = value; }
    public int MechCurrentEnergy { get => mechCurrentEnergy; set => mechCurrentEnergy = value; }

    public MechObject(MechComponentDataObject head, MechComponentDataObject torso, MechComponentDataObject arms, MechComponentDataObject legs)
    {
        mechHead = head;
        mechTorso = torso;
        mechArms = arms;
        mechLegs = legs;

        mechMaxHP += head.ComponentMaxHP;
        mechMaxHP += torso.ComponentMaxHP;
        mechMaxHP += arms.ComponentMaxHP;
        mechMaxHP += legs.ComponentMaxHP;

        mechMaxEnergy += head.ComponentMaxEnergy;
        mechMaxEnergy += torso.ComponentMaxEnergy;
        mechMaxEnergy += arms.ComponentMaxEnergy;
        mechMaxEnergy += legs.ComponentMaxEnergy;

        mechCurrentHP = mechMaxHP;
        mechCurrentEnergy = mechMaxEnergy;

        mechEnergyGain += head.EnergyGainModifier;
        mechEnergyGain += torso.EnergyGainModifier;
        mechEnergyGain += arms.EnergyGainModifier;
        mechEnergyGain += legs.EnergyGainModifier;
    }

    public MechComponentDataObject ReplaceComponent(MechComponentDataObject newComponent)
    {
        MechComponentDataObject oldComponent;
        Debug.Log("Replacing component.");
        switch (newComponent.ComponentType)
        {
            case MechComponent.Head:
                oldComponent = mechHead;
                mechHead = newComponent;

                RemoveMechHPByComponent(oldComponent);
                AddMechHPByComponent(newComponent);

                return oldComponent;

            case MechComponent.Torso:
                oldComponent = mechTorso;
                mechTorso = newComponent;

                RemoveMechHPByComponent(oldComponent);
                AddMechHPByComponent(newComponent);
                
                return oldComponent;

            case MechComponent.Arms:
                oldComponent = mechArms;
                mechArms = newComponent;

                RemoveMechHPByComponent(oldComponent);
                AddMechHPByComponent(newComponent);

                return oldComponent;

            case MechComponent.Legs:
                oldComponent = mechLegs;
                mechLegs = newComponent;

                RemoveMechHPByComponent(oldComponent);
                AddMechHPByComponent(newComponent);

                return oldComponent;
               
            case MechComponent.Back:
                return null;
        }

        return null;
    }

    public void DamageComponentHP(int damageToDeal, MechComponent component)
    {
        int bonusDamage = 0;

        switch (component)
        {
            case MechComponent.Torso:
                if (mechTorso.ComponentCurrentHP >= 0)
                {
                    mechTorso.ComponentCurrentHP -= damageToDeal;

                    if (mechTorso.ComponentCurrentHP < 0)
                        bonusDamage = Mathf.Abs(mechTorso.ComponentCurrentHP);
                    break;
                }
                else
                {
                    bonusDamage = damageToDeal;
                    break;
                }

            case MechComponent.Arms:
                if (mechArms.ComponentCurrentHP >= 0)
                {
                    mechArms.ComponentCurrentHP -= damageToDeal;

                    if (mechTorso.ComponentCurrentHP < 0)
                        bonusDamage = Mathf.Abs(mechArms.ComponentCurrentHP);
                    break;
                }
                else
                {
                    bonusDamage = damageToDeal;
                    break;
                }

            case MechComponent.Legs:
                if (mechLegs.ComponentCurrentHP >= 0)
                {
                    mechLegs.ComponentCurrentHP -= damageToDeal;

                    if (mechLegs.ComponentCurrentHP < 0)
                        bonusDamage = Mathf.Abs(mechLegs.ComponentCurrentHP);
                    break;
                }
                else
                {
                    bonusDamage = damageToDeal;
                    break;
                }

            case MechComponent.None:
                break;
            case MechComponent.Head:
                break;
            case MechComponent.Back:
                break;
        }

        DamageWholeMechHP(damageToDeal - bonusDamage);
        DamageWholeMechHP(Mathf.RoundToInt(bonusDamage * CombatManager.instance.BrokenCDM));
    }


    public void DamageWholeMechHP(int damageToDeal)
    {
        mechCurrentHP -= damageToDeal;
    }

    public void ResetHealth()
    {
        mechHead.HealComponent();
        mechTorso.HealComponent();
        mechArms.HealComponent();
        mechLegs.HealComponent();

        mechCurrentHP = mechMaxHP;
        ResetEnergy();
    }

    public void ResetEnergy()
    {
        mechCurrentEnergy = mechMaxEnergy;
    }

    private void RemoveMechHPByComponent(MechComponentDataObject componentRemoved)
    {
        mechMaxHP -= componentRemoved.ComponentMaxHP;
        mechCurrentHP -= componentRemoved.ComponentCurrentHP;
    }

    private void AddMechHPByComponent(MechComponentDataObject componentAdded)
    {
        mechMaxHP += componentAdded.ComponentMaxHP;
        mechCurrentHP += componentAdded.ComponentCurrentHP;
    }
}
