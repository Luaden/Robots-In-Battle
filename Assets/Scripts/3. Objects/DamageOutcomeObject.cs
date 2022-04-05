public class DamageOutcomeObject
{
    public int mechDamage;
    public int componentDamage;
    public MechComponent componentDamaged;

    public DamageOutcomeObject(int mechDamage, int componentDamage, MechComponent componentDamaged)
    {
        this.mechDamage = mechDamage;
        this.componentDamage = componentDamage;
        this.componentDamaged = componentDamaged;
    }
}
