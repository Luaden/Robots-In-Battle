using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMechComponent", menuName = "Mech Components/NewComponent", order = 0)]
[System.Serializable]
public class SOMechComponent : ScriptableObject
{
    [Header("Base Component Attributes")]
    [SerializeField] private string componentName;
    [SerializeField] private MechComponent componentType;
    [SerializeField] private Sprite componentSprite;
    [SerializeField] private int componentHP;
    [SerializeField] private int componentEnergy;
    [Range(1, 100)][SerializeField] private int chanceToSpawn;
    
    [Header("Component Effects")]
    [Tooltip("Component applies one stack of this element when an attack is used that utilizes this component.")]
    [SerializeField] private ElementType componentElement;
    [Tooltip("Component gives bonus damage to an attack that utilizes this component.")]
    [SerializeField] private int bonusDamageFromComponent;
    [Tooltip("Treat bonus damage to an attack that utilizes this component as a percentage of base damage.")]
    [SerializeField] private bool bonusDamageAsPercent;
    [Tooltip("Component takes reduced damage from an attack that targets this component.")]
    [SerializeField] private int reduceDamageToComponent;
    [Tooltip("Treat incoming damage reduction to this component as a percentage of base damage.")]
    [SerializeField] private bool reduceDamageAsPercent;
    [Tooltip("Component gives bonus element stacks to an attack that utilizes this component. This includes element stacks from cards as well as this component.")]
    [SerializeField] private int extraElementStacks;
    [Tooltip("Increases overall energy gained at the start of the turn.")]
    [SerializeField] private int energyGainModifier;


    public string ComponentName { get => componentName; }
    public MechComponent ComponentType { get => componentType; }
    public Sprite ComponentSprite { get => componentSprite; }
    public int ComponentHP { get => componentHP; }
    public int ComponentEnergy { get => componentEnergy; }
    public ElementType ComponentElement { get => componentElement; }
    public int BonusDamageFromComponent { get => bonusDamageFromComponent; }
    public bool BonusDamageAsPercent { get => bonusDamageAsPercent; }
    public int ReduceDamageToComponent { get => reduceDamageToComponent; }
    public bool ReduceDamageAsPercent { get => reduceDamageAsPercent; }
    public int ExtraElementStacks { get => extraElementStacks; }
    public int EnergyGainModifier { get => energyGainModifier; }

}
