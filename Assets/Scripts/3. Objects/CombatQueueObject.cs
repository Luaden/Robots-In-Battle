using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatQueueObject
{
    public Queue<DamageMechPairObject> damageQueue = new Queue<DamageMechPairObject>();
    public Queue<AnimationQueueObject> animationQueue = new Queue<AnimationQueueObject>();
    public CardBurnObject cardBurnObject = new CardBurnObject();
    public EnergyRemovalObject energyRemovalObject = new EnergyRemovalObject();
}
